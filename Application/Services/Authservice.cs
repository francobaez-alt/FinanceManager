using Application.DTOs.Users;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Security;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Models;
using FluentValidation;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMapper _mapper;
    private readonly IValidator<RegisterUserDto> _registerValidator;
    private readonly IValidator<LoginUserDto> _loginValidator;

    public AuthService(
        IUserRepository userRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        IPasswordHasher passwordHasher,
        IMapper mapper,
        IValidator<LoginUserDto> loginValidator,
        IValidator<RegisterUserDto> registerValidator)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
        _loginValidator = loginValidator;
        _registerValidator = registerValidator;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterUserDto dto)
    {
        await ValidateRegisterUserDtoAsync(dto);

        await EnsureUserDoesNotExistAsync(dto.Email);

        var user = CreateUser(dto);

        await SaveUserAsync(user);

        return await CreateAuthResponseAsync(user.Email);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginUserDto dto)
    {
        await ValidateLoginUserDtoAsync(dto);
        var user = await AuthenticateUserAsync(dto);
        return await CreateAuthResponseAsync(user.Email);
    }

    private async Task ValidateRegisterUserDtoAsync(RegisterUserDto dto)
    {
        await _registerValidator.ValidateAndThrowAsync(dto);
    }

    private async Task ValidateLoginUserDtoAsync(LoginUserDto dto)
    {
        await _loginValidator.ValidateAndThrowAsync(dto);
    }

    private async Task EnsureUserDoesNotExistAsync(string email)
    {
        if (await _userRepository.GetByEmailAsync(email) != null)
            throw new UserAlreadyExistsException(email);
    }

    private User CreateUser(RegisterUserDto dto)
    {
        var user = _mapper.Map<User>(dto);

        user.PasswordHash = _passwordHasher.Hash(dto.Password);
        user.RoleId = 1;
        user.IsActive = true;

        return user;
    }

    private async Task<AuthResponseDto> CreateAuthResponseAsync(string email)
    {
        var user = await _userRepository.GetByEmailWithPermissionsAsync(email)
            ?? throw new NotFoundException("User not found");

        if (user.Role == null)
            throw new NotFoundException($"Role is null. RoleId = {user.RoleId}");

        var response = _mapper.Map<AuthResponseDto>(user);
        response.Token = _jwtTokenGenerator.GenerateToken(user);

        return response;
    }
    private async Task<User> AuthenticateUserAsync(LoginUserDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);

        if (user == null)
            throw new UnauthorizedException("Invalid credentials");

        if (!_passwordHasher.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedException("Invalid credentials");

        if (!user.IsActive)
            throw new BusinessException("User is banned");

        return user;
    }

    private async Task SaveUserAsync(User user)
    {
        await _userRepository.Add(user);
        await _userRepository.Save();
    }

}