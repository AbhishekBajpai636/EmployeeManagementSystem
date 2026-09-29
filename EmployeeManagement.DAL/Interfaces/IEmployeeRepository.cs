using EmployeeManagement.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.DAL.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync();

        Task<EmployeeDTO?> GetEmployeeByIdAsync(int id);

        Task<int> CreateEmployeeAsync(EmployeeDTO employee);

        Task<bool> UpdateEmployeeAsync(EmployeeDTO employee);

        Task<bool> DeleteEmployeeAsync(int id);
    }
}
