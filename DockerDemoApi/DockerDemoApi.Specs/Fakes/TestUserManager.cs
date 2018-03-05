using System.Threading.Tasks;
using DockerDemoApi.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DockerDemoApi.Specs.Fakes
{
    public class TestUserManager : UserManager<User>
    {
        public TestUserManager() : base(new UserStore<User>(new DbContext(new DbContextOptionsBuilder().Options)), null, null, null, null, null, null, null, null)
        {
        }

        public override async Task<User> FindByNameAsync(string userName)
        {
            return await Task.Run(() => new User
            {
                Email = "test@user.com"
            });
        }
    }
}
