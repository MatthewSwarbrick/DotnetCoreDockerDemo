using DockerDemoApi.Domain;
using Microsoft.AspNetCore.Identity;

namespace DockerDemoApi.Orm
{
    public class Seeder
    {
        public static async void SeedData(UserManager<User> userManager)
        {
            if (userManager.FindByNameAsync("MSwarbrick").Result == null)
            {
                var user = new User
                {
                    UserName = "DemoUser",
                    Email = "demo@acme.com"
                };

                await userManager.CreateAsync(user, "1Strongpassword!");
            }
        }

    }
}
