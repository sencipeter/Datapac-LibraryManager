
using LibraryManger.Core.Interfaces;
using LibraryManger.Core.Interfaces.Data;
using LibraryManger.Infrastructure;
using LibraryManger.Infrastructure.Services;
using LibraryManger.Models.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryManger.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("AppConnectionString");
            builder.Services
                .AddDbContext<AppDbContext>(b =>
                {
                    b.UseSqlServer(connectionString);
                });
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddScoped(typeof(IAppRepository<>), typeof(AppRepository<>));
            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddScoped<IBorrowingBookService, BorrowingBookService>();


            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            AppDbContext dbcontext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbcontext.Database.Migrate();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
