using EmployeeManagement.Common.DTOs;
using EmployeeManagement.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.BAL.Interfaces
{
    public interface IAuthentication
    {
        Task<User?> LoginAsync(LoginRequestDTO request);

        Task<int> RegisterAsync(RegisterRequestDTO request);
    }   
}
