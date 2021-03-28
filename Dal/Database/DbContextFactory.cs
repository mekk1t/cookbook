using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace KitProjects.MasterChef.Dal.Database
{
    public class DbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        AppDbContext IDesignTimeDbContextFactory<AppDbContext>.CreateDbContext(string[] args)
        {
            var connectionString = args[0];
            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("Пустая строка подключения");
                throw new ArgumentNullException(nameof(connectionString), "Строка подключения была пустой");
            }
            Console.WriteLine($"Строка подключения: \n{connectionString}");
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
