using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace KitProjects.Cookbook.Database
{
    public class DbContextFactory : IDesignTimeDbContextFactory<CookbookDbContext>
    {
        public CookbookDbContext CreateDbContext(string[] args)
        {
            if (args.Length == 0)
                throw new ArgumentException("Отсутствуют параметры командной строки.");

            var connectionString = args[0];
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString), "Строка подключения была пустой");

            Console.WriteLine($"Строка подключения: \n{connectionString}\n");
            var optionsBuilder = new DbContextOptionsBuilder<CookbookDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new CookbookDbContext(optionsBuilder.Options);
        }
    }
}