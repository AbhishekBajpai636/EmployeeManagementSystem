using EmployeeManagement.Common.Models;
using EmployeeManagement.DAL.Data;
using EmployeeManagement.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.DAL.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IDbConnectionFactory _dbconnectionfactory;
        private readonly ILogger<AuthRepository> _logger;
        public AuthRepository(IDbConnectionFactory dbconnectionfactory,ILogger<AuthRepository> logger)
        {
            _dbconnectionfactory = dbconnectionfactory;
            _logger = logger;
        }
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            _logger.LogInformation("Fetching user by email: {Email}", email);
            using var connection = _dbconnectionfactory.CreateConnection();

            string sql =
                @"SELECT * From Users
                  WHERE Email = @Email";
            _logger.LogInformation("Executing SQL query to fetch user by email: {Email}", email);
            return await connection.QueryFirstOrDefaultAsync<User>(sql,new { Email = email });
        }
        public async Task<int> CreateUserAsync(User user)
        {
            using var connection = _dbconnectionfactory.CreateConnection();
            _logger.LogInformation("Creating a new user with email: {Email}", user.Email);  
            string sql =
            @"INSERT INTO Users
            (
                Name,
                Email,
                PasswordHash,
                Role
            )
            VALUES
            (
                @Name,
                @Email,
                @PasswordHash,
                @Role
            );

            SELECT CAST(SCOPE_IDENTITY() as int)";
            _logger.LogInformation("Executing SQL query to create user with email: {Email}", user.Email);
            return await connection.ExecuteScalarAsync<int>(
                sql,
                user);
        }
    }
}
