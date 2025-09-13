using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using yourself_demoAPI.Data;
using yourself_demoAPI.DTOs;
using yourself_demoAPI.Models.Auth;
using static yourself_demoAPI.Data.Helpers.Data;

namespace yourself_demoAPI.Repository.Dashboard
{
    public class DashboardRepo : IDashboardRepo
    {
        private readonly ApplicationDBContext _context;
        public DashboardRepo(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Account?> AddSystemUser(SystemUserRegisterDTO request)
        {
            if (_context.Accounts.Any(a => a.Email == request.Email))
            {
                return null;
            }

            var accountId = Guid.NewGuid();

            var account = new Account
            {
                Id = accountId,
                Email = request.Email,
                Role = request.Role,
            };

            account.HashedPassword = new PasswordHasher<Account>().HashPassword(account, request.Password);

            switch (request.Role)
            {
                case Roles.Admin:
                    var adminId = Guid.NewGuid();

                    var admin = new Admin
                    {
                        Id = adminId,
                        FirstName = request.FirstName! ?? "",
                        LastName = request.LastName! ?? "",
                        AccountId = accountId
                    };

                    _context.Admins.Add(admin);
                    break;
                    //case Roles.Designer:
                    //    var designerId = Guid.NewGuid();

                    //    var designer = new Designer
                    //    {
                    //        Id = designerId,
                    //        FirstName = request.FirstName! ?? "",
                    //        LastName = request.LastName! ?? "",
                    //        AccountId = accountId
                    //    };
                    //    _context.Designers.Add(designer);
                    //    break;
            }

            _context.Accounts.Add(account);

            await _context.SaveChangesAsync();

            return account;
        }
    }
}
