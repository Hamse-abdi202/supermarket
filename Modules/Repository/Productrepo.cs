using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hospital.Modules.Models;

namespace Hospital.Modules.Repository
{
    public interface Productrepo
    {
        Task<IEnumerable<product>> GetProducts();

        Task<product> GetProductbyId(int id);

        Task<product> DeleteUser(int id);

        Task<product> Create(product product);      
    }
}