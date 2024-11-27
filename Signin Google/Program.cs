
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Signin_Google.Database;
using Signin_Google.Dtos;
using Signin_Google.Services;

namespace Signin_Google
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Bind Google settings
            builder.Services.Configure<GoogleAuthConfig>(
                builder.Configuration.GetSection("Google")
            );
            // Add database context and Identity
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();



            // Add scoped Google auth service
            builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();




















            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
        }
    }
}
