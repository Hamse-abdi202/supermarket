using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hospital.Data;
using Hospital.Modules.DTOs;
using Hospital.Modules.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Modules.Repository
{

    
    public class UserRepository : Userrepo
    {
        private readonly ApiDbContext _context;
public UserRepository(ApiDbContext context)
        {

            _context = context;
}
        public async Task<user> createUser(user user)
        {

            await  _context.Users.AddAsync(user);
           await  _context.SaveChangesAsync();

            return user;
        }

        public async Task<user> DeleteUser(int id)
        {

            var DeletedUser = await _context.Users.FindAsync(id);
            _context.Users.Remove(DeletedUser);
            await _context.SaveChangesAsync();
            return DeletedUser;
            }

        public async Task<user> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<user>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }


        public async Task<user?> Loginuse(LoginDto logindata)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == logindata.Email);

            if (existingUser == null)
            {
                return null;
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(
                logindata.Password,
                existingUser.Password
            );

            if (!isPasswordValid)
            {
                return null;
            }

            return existingUser;
        }

        public async Task<user?> UpdateUser(user user)
        {
            var existingUser = await _context.Users.FindAsync(user.id);

            if (existingUser == null)
            {
                return null;
            }

            _context.Entry(existingUser).CurrentValues.SetValues(user);

            await _context.SaveChangesAsync();

            return existingUser;
        }

    }
}