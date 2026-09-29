using Dapper;
using EmployeeManagement.Common.DTOs;
using EmployeeManagement.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.DAL.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IConfiguration _configuration;

        public EmployeeRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));
        }


        public async Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync()
        {
            using var connection = CreateConnection();

            return await connection.QueryAsync<EmployeeDTO>(
                "SP_Employee_GetAll",
                commandType: CommandType.StoredProcedure);
        }


        public async Task<EmployeeDTO?> GetEmployeeByIdAsync(int id)
        {
            using var connection = CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<EmployeeDTO>(
                "SP_Employee_GetById",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }


        public async Task<int> CreateEmployeeAsync(EmployeeDTO employee)
        {
            using var connection = CreateConnection();

            return await connection.ExecuteScalarAsync<int>(
                "SP_Employee_Insert",
                new
                {
                    employee.Name,
                    employee.Email,
                    employee.Phone,
                    employee.Department,
                    employee.Designation,
                    employee.JoiningDate,
                    employee.Salary,
                    employee.IsActive
                },
                commandType: CommandType.StoredProcedure);
        }


        public async Task<bool> UpdateEmployeeAsync(EmployeeDTO employee)
        {
            using var connection = CreateConnection();

            var rowsAffected = await connection.ExecuteScalarAsync<int>(
                "SP_Employee_Update",
                new
                {
                    employee.Id,
                    employee.Name,
                    employee.Email,
                    employee.Phone,
                    employee.Department,
                    employee.Designation,
                    employee.JoiningDate,
                    employee.Salary,
                    employee.IsActive
                },
                commandType: CommandType.StoredProcedure);

            return rowsAffected > 0;
        }


        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            using var connection = CreateConnection();

            var rowsAffected = await connection.ExecuteScalarAsync<int>(
                "SP_Employee_Delete",
                new { Id = id },
                commandType: CommandType.StoredProcedure);

            return rowsAffected > 0;
        }
    }
}
