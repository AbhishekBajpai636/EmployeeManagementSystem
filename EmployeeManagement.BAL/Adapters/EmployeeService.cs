using EmployeeManagement.BAL.Interfaces;
using EmployeeManagement.Common.DTOs;
using EmployeeManagement.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.BAL.Adapters
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync()
        {
            return await _employeeRepository.GetAllEmployeesAsync();
        }

        public async Task<EmployeeDTO?> GetEmployeeByIdAsync(int id)
        {
            return await _employeeRepository.GetEmployeeByIdAsync(id);
        }

        public async Task<int> CreateEmployeeAsync(EmployeeDTO employee)
        {
            return await _employeeRepository.CreateEmployeeAsync(employee);
        }

        public async Task<bool> UpdateEmployeeAsync(EmployeeDTO employee)
        {
            return await _employeeRepository.UpdateEmployeeAsync(employee);
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            return await _employeeRepository.DeleteEmployeeAsync(id);
        }
    }
}
