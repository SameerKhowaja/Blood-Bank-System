using BloodBankAPI_2.Service.IService;
using DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankAPI_2.Service
{
    public class DbInitializer: IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db,
            UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            try
            {
                /// Check if there are any pending migrations to DB
                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();
                }

                /// TODO: unharcode Role names by using enums
                if (_db.Roles.Any(role => role.Name == "Admin")) return;

                /// Roles don't exist so we should create them
                IdentityRole identityRole = new("Admin");
                _roleManager.CreateAsync(identityRole).GetAwaiter().GetResult();

                /// Can Add more roles here

                /// Create Default IdentityUser with the Roles above
                IdentityUser identityUser = new()
                {
                    UserName="admin@bloodbank2.com",
                    Email="admin@bloodbank2.com",
                    EmailConfirmed=true,
                };

                _userManager.CreateAsync(identityUser, password: "Admin2!").GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(identityUser, "Admin").GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
