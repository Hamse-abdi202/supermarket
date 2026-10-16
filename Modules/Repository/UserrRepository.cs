
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Hospital.Data;
using Hospital.Modules.DTOs;
using Hospital.Modules.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Hospital.Modules.Repository
{
    public class UserrRepository : Userrepo
    {
        private readonly ApiDbContext _context;
        private readonly ILogger<UserrRepository> _logger;
        private readonly IConfiguration _configuration;

        public UserrRepository(
            ApiDbContext context,
            ILogger<UserrRepository> logger,
            IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto?> Login(LoginDto loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.Email) ||
                string.IsNullOrWhiteSpace(loginDto.Password))
            {
                return null;
            }

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (existingUser == null)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(existingUser.Password))
            {
                _logger.LogWarning(
                    "Password is missing for user ID {UserId}",
                    existingUser.id);

                return null;
            }

            bool isPasswordValid;

            try
            {
                isPasswordValid = BCrypt.Net.BCrypt.Verify(
                    loginDto.Password,
                    existingUser.Password);
            }
            catch (ArgumentException)
            {
                _logger.LogWarning(
                    "Invalid password hash for user ID {UserId}",
                    existingUser.id);

                return null;
            }

            if (!isPasswordValid)
            {
                return null;
            }

            var jwtKey = _configuration["Jwt:Key"];

            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                throw new InvalidOperationException(
                    "JWT key is missing from configuration.");
            }

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    existingUser.id.ToString()),

                new Claim(
                    ClaimTypes.Email,
                    existingUser.Email),

                new Claim(
                    ClaimTypes.Name,
                    $"{existingUser.Firstname} {existingUser.Lastname}")
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentials);

            var accessToken = new JwtSecurityTokenHandler()
                .WriteToken(token);

            var refreshToken = Convert.ToBase64String(
                RandomNumberGenerator.GetBytes(64));

            existingUser.RefreshToken = refreshToken;
            existingUser.RefreshTokenExpiryTime =
                DateTime.UtcNow.AddDays(7);

            await _context.SaveChangesAsync();

            return new LoginResponseDto
            {
                UserId = existingUser.id,
                Email = existingUser.Email,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<IEnumerable<user>> Createuser(user user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return new List<user> { user };
        }

        public async Task<IEnumerable<user>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<IEnumerable<user>> GetUsersByid(int id)
        {
            return await _context.Users
                .Where(u => u.id == id)
                .ToListAsync();
        }

        public async Task<IEnumerable<user>> update(user user)
        {
            _context.Users.Update(user);

            await _context.SaveChangesAsync();

            return new List<user> { user };
        }

        public async Task<IEnumerable<user>> DeleteUser(int id)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.id == id);

            if (existingUser == null)
            {
                return Enumerable.Empty<user>();
            }

            _context.Users.Remove(existingUser);

            await _context.SaveChangesAsync();

            return new List<user> { existingUser };
        }

        Task<user> Userrepo.Createuser(user user)
        {
            throw new NotImplementedException();
        }

        Task<user> Userrepo.GetUsersByid(int id)
        {
            throw new NotImplementedException();
        }

        Task<user> Userrepo.update(user user)
        {
            throw new NotImplementedException();
        }

        Task<user> Userrepo.DeleteUser(int id)
        {
            throw new NotImplementedException();
        }
    }
}
