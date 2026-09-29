using EmployeeManagement.BAL.Interfaces;
using EmployeeManagement.Common.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET: api/Employee
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();

            return Ok(employees);
        }

        // GET: api/Employee/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        // POST: api/Employee
        [HttpPost]
        public async Task<IActionResult> CreateEmployee(EmployeeDTO employee)
        {
            var id = await _employeeService.CreateEmployeeAsync(employee);

            return Ok(new
            {
                success = true,
                message = "Employee created successfully.",
                id = id
            });
        }

        // PUT: api/Employee/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(
            int id,
            EmployeeDTO employee)
        {
            employee.Id = id;

            var result = await _employeeService.UpdateEmployeeAsync(employee);

            if (!result)
                return NotFound();

            return Ok(new
            {
                success = true,
                message = "Employee updated successfully."
            });
        }

        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var result = await _employeeService.DeleteEmployeeAsync(id);

            if (!result)
                return NotFound();

            return Ok(new
            {
                success = true,
                message = "Employee deleted successfully."
            });
        }
        [HttpGet("test-exception")]
        public IActionResult TestException()
        {
            throw new Exception("Test exception");
        }
    }
}
