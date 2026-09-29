using EmployeeManagement.Common.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagement.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO request)
        {
            _httpClient.BaseAddress =
                new Uri("https://localhost:7121/");

            var response = await _httpClient.PostAsJsonAsync(
                "api/auth/login",
                request);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Invalid Email or Password";
                return View(request);
            }

            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
            // why do we need to read from json async? because the response content is in json format and we need to deserialize it into a C# object
            HttpContext.Session.SetString(
                "JWToken",
                loginResponse.Token);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, request.Email)
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);

            return RedirectToAction(
                "Index",
                "Home");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile() {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login");
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _httpClient.BaseAddress =
                new Uri("https://localhost:7121/");

            var response = await _httpClient.GetAsync(
                "api/auth/profile");
            Console.WriteLine(response.StatusCode);
            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }
            var profileResponse = await response.Content.ReadFromJsonAsync<ProfileResponseDTO>();

            ViewBag.UserId = profileResponse.UserId;
            ViewBag.Email = profileResponse.Email;
            ViewBag.Role = profileResponse.Role;

            return View(profileResponse);

        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestDTO request)
        {
            _httpClient.BaseAddress =
                new Uri("https://localhost:7121/");

            var response = await _httpClient.PostAsJsonAsync(
                "api/auth/register",
                request);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Registration Failed";

                return View(request);
            }

            TempData["Success"] = "Registration Successful. Please Login.";

            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync(
        CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}