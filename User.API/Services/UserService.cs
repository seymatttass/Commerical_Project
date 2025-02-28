using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Users.API.Data.Entities;
using Users.API.Data.Repository;
using Users.API.DTOS.Users;

namespace Users.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Userss> AddAsync(CreateUserDTO createUserDto)
        {
            try
            {
                var user = _mapper.Map<Userss>(createUserDto);
                user.CreatedDate = DateTime.UtcNow;

                await _userRepository.AddAsync(user);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating user");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await _userRepository.RemoveAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting user {id}");
                throw;
            }
        }

        public async Task<IEnumerable<Userss>> GetAllAsync()
        {
            try
            {
                return await _userRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all users");
                throw;
            }
        }

        public async Task<Userss> GetByIdAsync(int id)
        {
            try
            {
                return await _userRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while getting user {id}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(UpdateUserDTO updateUserDto)
        {
            try
            {
                var user = _mapper.Map<Userss>(updateUserDto);
                user.CreatedDate = DateTime.UtcNow; // Güncelleme zamanını kaydetmek için

                return await _userRepository.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating user {updateUserDto.Id}");
                throw;
            }
        }

        public async Task<Userss?> AuthenticateAsync(string username, string password)
        {
            try
            {
                var user = await _userRepository.AuthenticateAsync(username, password);
                if (user == null)
                {
                    _logger.LogWarning($"Authentication failed for user {username}");
                }
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while authenticating user {username}");
                throw;
            }
        }
    }
}
