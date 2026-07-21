using Application.Common;
using Application.DTOs.Users;
using Application.Interfaces.Repositories;
using Application.Interfaces.Security;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Models;
using Microsoft.Extensions.Logging;
using System.Net;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMapper _mapper;
    private readonly ILogger<AuthService> _logger;  

    public AuthService(
        IUserRepository userRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        IPasswordHasher passwordHasher,
        IMapper mapper,
        ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterUserDto dto)
    {
        if (await _userRepository.ExistEmailAsync(dto.Email))
        {
            _logger.LogWarning("User registration failed. Email already exists. Email: {dto.Email}", dto.Email);
            return ApiResponse<AuthResponseDto>.Fail("User already exists.");
        }

        var user = CreateUser(dto);

        await SaveUserAsync(user);

        var response = await CreateAuthResponseAsync(user.Email);

        if (response == null)
        {
            _logger.LogError("Failed to create authentication response for user. Email: {user.Email}", user.Email);
            return ApiResponse<AuthResponseDto>.Fail(
                "Unable to create authentication response.",
                HttpStatusCode.InternalServerError);
        }

        _logger.LogInformation("User registered successfully. Email: {user.Email}", user.Email);
        return ApiResponse<AuthResponseDto>.Ok(
            response,
            "User registered successfully.",
            HttpStatusCode.Created);
    }

    public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginUserDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);

        if (user == null)
        {
            _logger.LogWarning("Login failed. User not found. Email: {dto.Email}", dto.Email);
            return ApiResponse<AuthResponseDto>.Fail("Invalid credentials.", HttpStatusCode.Unauthorized);
        }

        if (!_passwordHasher.Verify(dto.Password, user.PasswordHash))
        {
            _logger.LogWarning("Login failed. Invalid password. Email: {dto.Email}", dto.Email);
            return ApiResponse<AuthResponseDto>.Fail("Invalid credentials.", HttpStatusCode.Unauthorized);
        }

        if (!user.IsActive)
        {
            _logger.LogWarning("Login failed. User is banned. Email: {dto.Email}", dto.Email);
            return ApiResponse<AuthResponseDto>.Fail("User is banned.", HttpStatusCode.Forbidden);
        }

        var response = await CreateAuthResponseAsync(user.Email);

        if (response == null)
        {
            _logger.LogError("Failed to create authentication response for user. Email: {user.Email}", user.Email);
            return ApiResponse<AuthResponseDto>.Fail(
                "Unable to create authentication response.",
                HttpStatusCode.InternalServerError);
        }

        _logger.LogInformation("Login successful. Email: {user.Email}", user.Email);
        return ApiResponse<AuthResponseDto>.Ok(
            response,
            "Login successful.",
            HttpStatusCode.OK);
    }

    private User CreateUser(RegisterUserDto dto)
    {
        var user = _mapper.Map<User>(dto);

        user.PasswordHash = _passwordHasher.Hash(dto.Password);
        user.RoleId = 2;
        user.IsActive = true;

        return user;
    }

    private async Task<AuthResponseDto?> CreateAuthResponseAsync(string email)
    {
        var user = await _userRepository.GetByEmailWithPermissionsAsync(email);

        if (user == null)
            return null;

        if (user.Role?.RolePermissions?.Any() != true)
        {
            throw new InvalidOperationException(
                $"User {user.Id} has an invalid role configuration.");
        }

        var response = _mapper.Map<AuthResponseDto>(user);
        response.Token = _jwtTokenGenerator.GenerateToken(user);

        return response;
    }

    private async Task SaveUserAsync(User user)
    {
        await _userRepository.Add(user);
        await _userRepository.Save();
    }
}