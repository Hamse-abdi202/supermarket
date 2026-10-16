using System.Collections.Generic;
using System.Threading.Tasks;
using Hospital.Modules.DTOs;
using Hospital.Modules.Models;
using Hospital.Modules.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Modules.controlers
{
    [ApiController]
    [Route("api/[controller]")]
    public class userApi : ControllerBase
    {
        private readonly Userrepo _IuserRepo;

        public userApi(Userrepo IuserRepo)
        {
            _IuserRepo = IuserRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<user>>> GetUsers()
        {
            var users = await _IuserRepo.GetUsers();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<user>> GetUsersById(int id)
        {
            var existingUser = await _IuserRepo.GetUsersByid(id);

            if (existingUser == null)
            {
                return NotFound(new
                {
                    message = "User not found"
                });
            }

            return Ok(existingUser);
        }

        [HttpPost]
        public async Task<ActionResult<user>> CreateUser([FromBody] user user)
        {
            var newUser = await _IuserRepo.Createuser(user);

            return CreatedAtAction(
                nameof(GetUsersById),
                new { id = newUser.id },
                newUser
            );
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _IuserRepo.Login(loginDto);

            if (result == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password"
                });
            }

            return Ok(result);
        }
    }
}