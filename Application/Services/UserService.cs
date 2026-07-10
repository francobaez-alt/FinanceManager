using Application.DTOs.Users;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdWithRoleAsync(id);

            if (user is null)
                throw new Exception("User not found");

            return _mapper.Map<UserDto>(user);
        }
    }
}
