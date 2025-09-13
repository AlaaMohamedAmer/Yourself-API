
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using Yourself_App.Repository.Profile;
using yourself_demoAPI.Data;
using yourself_demoAPI.Data.Helpers;
using yourself_demoAPI.Repository.Auth;
using yourself_demoAPI.Repository.Collection;
using yourself_demoAPI.Repository.Dashboard;
using yourself_demoAPI.Repository.EmailSender;
using yourself_demoAPI.Repository.Home;
using yourself_demoAPI.Repository.Search;

namespace yourself_demoAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration.GetValue<string>("AppSettings:Issuer"),
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration.GetValue<string>("AppSettings:Audience"),
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("AppSettings:Token")!)),
                };
            });
            
            builder.Services.AddScoped<IAuthRepo, AuthRepo>();
            builder.Services.AddScoped<IDashboardRepo, DashboardRepo>();
			builder.Services.AddTransient<ISmtpEmailSender, SmtpEmailSender>();
            builder.Services.AddScoped<IProfileService,ProfileService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ICollectionServices,CollectionService>();
            builder.Services.AddScoped<ISearchService,SearchService>();
            builder.Services.AddHttpContextAccessor();

			builder.Services.AddControllers()
	                .AddJsonOptions(options =>
	                {
		                options.JsonSerializerOptions.DefaultIgnoreCondition =
		                System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
	                });
			builder.Services.AddOpenApi();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                await DBSeeder.SeedSuperAdminAsync(dbContext, configuration);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
