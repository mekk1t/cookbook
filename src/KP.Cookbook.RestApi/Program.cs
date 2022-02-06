using KP.Cookbook.Database;
using KP.Cookbook.RestApi.Services;
using KP.Cookbook.Uow;
using Npgsql;
using System.Data.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var services = builder.Services;

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddScoped<Func<DbConnection>>(serviceProvider =>
    () => new NpgsqlConnection(builder.Configuration.GetConnectionString("Postgresql")));

services.AddScoped<IngredientsRepository>();
services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UnitOfWork>());
services.AddScoped<IngredientsService>();
services.AddScoped<UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
