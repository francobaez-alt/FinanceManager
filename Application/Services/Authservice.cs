using Application.DTOs.Users;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Security;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Models;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMapper _mapper;

    public AuthService(
        IUserRepository userRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        IPasswordHasher passwordHasher,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterUserDto dto)
    {
        var existingUser = await _userRepository.GetByEmailAsync(dto.Email);

        if (existingUser != null)
            throw new UserAlreadyExistsException(dto.Email);

        var user = _mapper.Map<User>(dto);

        user.PasswordHash = _passwordHasher.Hash(dto.Password);

        // reglas de negocio (NO AutoMapper)
        user.RoleId = 1; // user default
        user.IsActive = true;
        //user.IsEmailConfirmed = false;
        //user.CreatedAt = DateTime.UtcNow;

        await _userRepository.Add(user);
        await _userRepository.Save();


        var createdUser = await _userRepository.GetByEmailAsync(user.Email);

         if (createdUser == null)
             throw new NotFoundException("User not found");

        if (createdUser.Role == null)
            throw new NotFoundException($"Role is null. RoleId = {createdUser.RoleId}");

        var token = _jwtTokenGenerator.GenerateToken(createdUser!, createdUser!.Role.Name);

        var response = _mapper.Map<AuthResponseDto>(createdUser);
        response.Token = token;

        return response;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginUserDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);

        if (user == null)
            throw new NotFoundException("Invalid credentials");

        var isValidPassword = _passwordHasher.Verify(dto.Password, user.PasswordHash);

        if (!isValidPassword)
            throw new NotFoundException("Invalid credentials");

        if (!user.IsActive)
            throw new BusinessException("User is banned");

        var token = _jwtTokenGenerator.GenerateToken(user, user.Role.Name);

        var response = _mapper.Map<AuthResponseDto>(user);
        response.Token = token;

        return response;
    }
}