using CafeKDSApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeKDSApi.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) 
        {

        }

        public DbSet<Order> Order { get; set; }
        public DbSet<Item> Item {  get; set; }
    }
}
