using EmployeeManagement.BAL.Interfaces;
using EmployeeManagement.Common.DTOs;
using EmployeeManagement.Common.Models;
using EmployeeManagement.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

using Microsoft.Extensions.Logging;

namespace EmployeeManagement.BAL.Adapters
{
    public class AuthenticationAdapter : IAuthentication
    {
        private readonly IAuthRepository _authRepository;
        private readonly ILogger<AuthenticationAdapter> _logger;

        public AuthenticationAdapter(IAuthRepository authRepository, ILogger<AuthenticationAdapter> logger)
        {
            _authRepository = authRepository;
            _logger = logger;
        }
        public async Task<int> RegisterAsync(RegisterRequestDTO request)
        {
            _logger.LogInformation("adapter register request received.");
            var existingUser =await _authRepository.GetUserByEmailAsync(request.Email);

            if (existingUser != null)
            {
                throw new Exception("User already exists.");
            }

            string passwordHash =
                BCrypt.Net.BCrypt.HashPassword(
                    request.Password);

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = passwordHash,
                Role = "User"
            };
            _logger.LogInformation("Adapter User {Email} register in successfully.",
                                    request.Email);
            return await _authRepository.CreateUserAsync(user);
        }
        public async Task<User?> LoginAsync(LoginRequestDTO request)
        {
            _logger.LogInformation("adapter Login request received.");
            var user = await _authRepository.GetUserByEmailAsync(request.Email);

            if (user == null)
                return null;

            bool isValid = BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.PasswordHash);

            if (!isValid)
                return null;
            _logger.LogInformation("adapter User {Email} logged in successfully.",
                                    request.Email);
            return user;
        }
    }
}
