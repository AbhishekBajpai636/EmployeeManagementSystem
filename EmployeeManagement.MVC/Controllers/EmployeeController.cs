using EmployeeManagement.Common.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace EmployeeManagement.MVC.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly HttpClient _httpClient;

        public EmployeeController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7121/");
        }

        private bool SetToken()
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
                return false;

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            return true;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (!SetToken())
                return RedirectToAction("Login", "Account");

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!SetToken())
                return Unauthorized();

            var response = await _httpClient.GetAsync("api/employee");

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode);

            var employees =
                await response.Content.ReadFromJsonAsync<IEnumerable<EmployeeDTO>>();

            return Json(employees);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            if (!SetToken())
                return Unauthorized();

            var response =
                await _httpClient.GetAsync($"api/employee/{id}");

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode);

            var employee =
                await response.Content.ReadFromJsonAsync<EmployeeDTO>();

            return Json(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmployeeDTO employee)
        {
            if (!SetToken())
                return Unauthorized();

            var response =
                await _httpClient.PostAsJsonAsync("api/employee", employee);

            var result = await response.Content.ReadAsStringAsync();

            return new ContentResult
            {
                StatusCode = (int)response.StatusCode,
                Content = result,
                ContentType = "application/json"
            };
        }

        [HttpPut]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] EmployeeDTO employee)
        {
            if (!SetToken())
                return Unauthorized();

            employee.Id = id;

            var response =
                await _httpClient.PutAsJsonAsync(
                    $"api/employee/{id}",
                    employee);

            var result = await response.Content.ReadAsStringAsync();

            return new ContentResult
            {
                StatusCode = (int)response.StatusCode,
                Content = result,
                ContentType = "application/json"
            };
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (!SetToken())
                return Unauthorized();

            var response =
                await _httpClient.DeleteAsync($"api/employee/{id}");

            var result = await response.Content.ReadAsStringAsync();

            return new ContentResult
            {
                StatusCode = (int)response.StatusCode,
                Content = result,
                ContentType = "application/json"
            };
        }
    }
}
