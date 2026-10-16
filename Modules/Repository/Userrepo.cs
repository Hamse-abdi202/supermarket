using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hospital.Modules.Models;

namespace Hospital.Modules.Repository
{
    public interface Userrepo
    {
        Task<IEnumerable<user>> GetUsers();
        Task<user> createUser(user user);

        Task<user> DeleteUser(int id);
        Task<user> GetUserById(int id);


        Task<user> UpdateUser(user user);

    }
}