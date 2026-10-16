
using Hospital.Modules.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Data
{
    public class ApiDbContext : DbContext
    {
        public virtual DbSet<user> Users { get; set; }
    

        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options)
        {
        }
    }
}
