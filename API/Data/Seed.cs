using API.Data;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace DatingApp.API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context)
        {
            // if (await userManager.Users.AnyAsync()) return;
            if (await context.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

            if (users == null) return;

            // var roles = new List<Appuser>
            //     {
            //         new() {Name = "Member"},
            //         new() {Name = "Admin"},
            //         new() {Name = "Moderator"},
            //     };

            // foreach (var role in roles)
            // {
            //     await roleManager.CreateAsync(role);
            // }

            foreach (var user in users)
            {
                using var hmac= new HMACSHA512();

                // user.Photos.First().IsApproved = true;
                user.UserName = user.UserName!.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                user.PasswordSalt = hmac.Key;

                context.Users.Add(user);

                // await userManager.CreateAsync(user, "Pa$$w0rd");
                // await userManager.AddToRoleAsync(user, "Member");
            }
            await context.SaveChangesAsync();

            // var admin = new AppUser
            // {
            //     UserName = "admin",
            //     KnownAs = "Admin",
            //     Gender = "",
            //     City = "",
            //     Country = ""
            // };

            // await userManager.CreateAsync(admin, "Pa$$w0rd");
            // await userManager.AddToRolesAsync(admin, ["Admin", "Moderator"]);
        }
    }
}