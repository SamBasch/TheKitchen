using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TheKitchen.Models;

namespace TheKitchen.Data
{
    public class DataUtility
    {
        private const string _adminRole = "Admin";

        private const string _adminEmail = "FrederickMeanstonAdmin@mailinator.com";


        public static DateTime GetPostGresDate(DateTime datetime)
        {
            return DateTime.SpecifyKind(datetime, DateTimeKind.Utc);

        }


        public static string GetConnectionString(IConfiguration configuration)
        {


            string? connectionString = configuration.GetConnectionString("DefaultConnection");
            string? databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            return string.IsNullOrEmpty(databaseUrl) ? connectionString! : BuildConnectionString(databaseUrl);

        }

        private static string BuildConnectionString(string databaseUrl)
        {
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');
            var builder = new NpgsqlConnectionStringBuilder()
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Require,
                TrustServerCertificate = true
            };
            return builder.ToString();
        }



        public static async Task ManageDataAsync(IServiceProvider svcProvider)
        {

            var dbContextSvc = svcProvider.GetRequiredService<ApplicationDbContext>();

            var configurationSvc = svcProvider.GetRequiredService<IConfiguration>();

            var userManagerSvc = svcProvider.GetRequiredService<UserManager<RBUser>>();

            var roleManagerSvc = svcProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await dbContextSvc.Database.MigrateAsync();


            //seed roles
            await SeedRolesAsync(roleManagerSvc);



            //seed users
            await SeedUsersAsync(dbContextSvc, configurationSvc, userManagerSvc);

        }


        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(_adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(_adminRole));
            }


        }



        private static async Task SeedUsersAsync(ApplicationDbContext context, IConfiguration configuration, UserManager<RBUser> userManager)
        {
            if (!context.Users.Any(u => u.Email == _adminEmail))
            {
                RBUser adminUser = new()
                {

                    Email = _adminEmail,
                    UserName = _adminEmail,
                    FirstName = "Sam",
                    LastName = "Baschnagel",
                    EmailConfirmed = true,

                };

                await userManager.CreateAsync(adminUser, configuration["AdminPwd"] ?? Environment.GetEnvironmentVariable("AdminPwd")!);
                await userManager.AddToRoleAsync(adminUser, _adminRole);

            }



        }

    }
}
