using KitProjects.MasterChef.Kernel.Models;
using Microsoft.EntityFrameworkCore;

namespace KitProjects.MasterChef.Dal
{
    public class AppDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
