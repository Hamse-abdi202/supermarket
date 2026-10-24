
using System;
using System.Threading.Tasks;
using Hospital.Modules.DTOs;
using Hospital.Modules.Models;
using Hospital.Modules.Repository;
using Hospital.Modules.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hospital.Modules.controlers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserApi : ControllerBase
    {
        private readonly Userrepo _IuserRepo;
        private readonly AuthService _authService;
        private readonly ILogger<UserApi> _logger;

        public UserApi(Userrepo IuserRepo,AuthService authService,ILogger<UserApi> logger)
        {
            _IuserRepo = IuserRepo;
            _authService = authService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<user>> GetUserByid(int id)
        {
            var existingUser = await _IuserRepo.GetUserById(id);

            if (existingUser == null)
            {
                _logger.LogWarning(
                    "User with ID {UserId} was not found", id);

                return NotFound("User not found");
            }

            return Ok(existingUser);
        }

        [HttpPost]
        public async Task<ActionResult<user>> Create(user user)
        {
            try
            {
                var newUser = await _IuserRepo.createUser(user);

                _logger.LogInformation(
                    "User with ID {UserId} created successfully",
                    newUser.id);

                return CreatedAtAction(
                    nameof(GetUserByid),
                    new { id = newUser.id },
                    newUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a user");

                return StatusCode(
                    500,
                    "An unexpected error occurred");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                var tokens = await _authService.LoginAsync(loginDto);

                if (tokens == null)
                {
                    _logger.LogWarning("Login failed");

                    return Unauthorized("Invalid email or password");
                }

                _logger.LogInformation("User logged in successfully");

                return Ok(tokens);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login");

                return StatusCode(
                    500,
                    "An unexpected error occurred");
            }
        }
    }
}
