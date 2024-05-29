using CafeKDSApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeKDSApi.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) 
        {

        }

        public DbSet<Orders> Orders { get; set; }
    }
}
