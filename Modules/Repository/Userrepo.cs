using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hospital.Modules.DTOs;
using Hospital.Modules.Models;

namespace Hospital.Modules.Repository
{
    public interface Userrepo
    {
        Task<IEnumerable<user>> GetUsers();
        Task<user> Createuser(user user);

        Task<user> GetUsersByid(int id);

        Task<user> update(user user);
        Task<user> DeleteUser(int id);

        Task<LoginResponseDto?> Login(LoginDto loginDto);

    }
}