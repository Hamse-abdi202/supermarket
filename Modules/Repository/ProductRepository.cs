using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hospital.Data;
using Hospital.Modules.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Modules.Repository
{
    public class ProductRepository : Productrepo
    {

        private readonly ApiDbContext _context;
        public ProductRepository(ApiDbContext context)
        {
            _context = context;
        }


        public async Task<product> Create(product product)
        {

            var newproducts = _context.Products.Add(product);

           await _context.SaveChangesAsync();

           return product;

            // var products= _context.Products.Add(product);
            // // _contex.sac 
            // // _context.SaveChangesAsync()
            // _context.SaveChangesAsync();

            // return product;



        }

        public async Task<product> DeleteUser(int id)
        {

            var Deleteproduct = await _context.Products.FindAsync(id);
            _context.Products.Remove(Deleteproduct);
            await _context.SaveChangesAsync();
            return Deleteproduct;
        }

        public async Task<product> GetProductbyId(int id)
        {

            return await _context.Products.FindAsync(id);
        }

        public async Task<IEnumerable<product>> GetProducts()
        {

             var Listusres=  await _context.Products.ToListAsync();


            return Listusres;
        }
    }
}