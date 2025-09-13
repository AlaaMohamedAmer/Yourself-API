using Microsoft.AspNetCore.Identity;
using yourself_demoAPI.Models.Auth;
using static yourself_demoAPI.Data.Helpers.Data;

namespace yourself_demoAPI.Data.Helpers
{
    public static class DBSeeder
    {
        public static async Task SeedSuperAdminAsync(ApplicationDBContext context, IConfiguration configuration)
        {
            var hasher = new PasswordHasher<Account>();

            if (!context.Accounts.Any(a => a.Role == "SuperAdmin"))
            {
                var superAdminAccountId = Guid.NewGuid();
                var superAdminId = Guid.NewGuid();

                var email = configuration["SuperAdmin:email"];
                var password = configuration["SuperAdmin:password"];

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                    throw new InvalidOperationException("SuperAdmin credentials are not configured.");

                var account = new Account
                {
                    Id = superAdminAccountId,
                    Email = email,
                    Role = Roles.SuperAdmin
                };

                account.HashedPassword = hasher.HashPassword(account, password);

                var admin = new Admin
                {
                    Id = superAdminId,
                    FirstName = configuration["SuperAdmin:firstName"]! ?? "",
                    LastName = configuration["SuperAdmin:lastName"]! ?? "",
                    AccountId = superAdminAccountId
                };

                context.Accounts.Add(account);
                context.Admins.Add(admin);

                await context.SaveChangesAsync();
            }
        }
    }
}
