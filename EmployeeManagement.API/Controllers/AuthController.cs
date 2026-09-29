using EmployeeManagement.API.Helpers;
using EmployeeManagement.BAL.Interfaces;
using EmployeeManagement.Common.DTOs;
using EmployeeManagement.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthentication _authentication;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthentication authentication, JwtTokenGenerator jwtTokenGenerator, ILogger<AuthController> logger)
        {
            _authentication = authentication;
            _jwtTokenGenerator = jwtTokenGenerator;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDTO request)
        {
            try
            {
                _logger.LogInformation("api registratio request received.");
                var userId =
                await _authentication.RegisterAsync(request);
                _logger.LogInformation("api User {Email} registered successfully.",
                                        request.Email);
                return Ok(new
                {
                    Message = "User Registered Successfully",
                    UserId = userId
                });
            }
            catch (Exception ex) { 
                _logger.LogError(ex,"An error occurred during registration for user {Email}.", request.Email);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO request)
        {
            try
            {
                _logger.LogInformation("api Login request received.");
                var user = await _authentication.LoginAsync(request);

                if (user == null)
                {
                    return Unauthorized();
                }

                var token = _jwtTokenGenerator.GenerateToken(user);
                _logger.LogInformation("api User {Email} logged in successfully.",
                                        request.Email);
                return Ok(new LoginResponseDTO
                {
                    Token = token
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login for user {Email}.", request.Email);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [Authorize]
        [HttpGet("profile")]
        public IActionResult Profile()
        {
            return Ok(new
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Email = User.FindFirst(ClaimTypes.Email)?.Value,
                Role = User.FindFirst(ClaimTypes.Role)?.Value
            });
        }
    }
}
