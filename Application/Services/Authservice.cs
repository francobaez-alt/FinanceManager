using Application.DTOs.Users;
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
            throw new Exception("User already exists");

        var user = _mapper.Map<User>(dto);

        user.PasswordHash = _passwordHasher.Hash(dto.Password);

        // reglas de negocio (NO AutoMapper)
        user.RoleId = 2; // user default
        user.IsActive = true;
        user.IsEmailConfirmed = false;
        user.CreatedAt = DateTime.UtcNow;

        await _userRepository.Add(user);
        await _userRepository.Save();


        // recargar con Role incluido (IMPORTANTE para JWT)
        var createdUser = await _userRepository.GetByEmailAsync(user.Email);

         if (createdUser == null)
             throw new Exception("User not found");

        if (createdUser.Role == null)
            throw new Exception($"Role is null. RoleId = {createdUser.RoleId}");

        var token = _jwtTokenGenerator.GenerateToken(createdUser!, createdUser!.Role.Name);

        var response = _mapper.Map<AuthResponseDto>(createdUser);
        response.Token = token;

        return response;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginUserDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);

        if (user == null)
            throw new Exception("Invalid credentials");

        var isValidPassword = _passwordHasher.Verify(dto.Password, user.PasswordHash);

        if (!isValidPassword)
            throw new Exception("Invalid credentials");

        if (!user.IsActive)
            throw new Exception("User is inactive");

        var token = _jwtTokenGenerator.GenerateToken(user, user.Role.Name);

        var response = _mapper.Map<AuthResponseDto>(user);
        response.Token = token;

        return response;
    }
}