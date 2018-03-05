using System;
using System.Threading.Tasks;
using DockerDemoApi.Domain;
using DockerDemoApi.Orm;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DockerDemoApi.Specs.Fakes
{
    public class TestUserManager : UserManager<User>
    {
        readonly ISession session;

        public TestUserManager(ISession session) : base(new UserStore<User>(new DbContext(new DbContextOptionsBuilder().Options)), null, null, null, null, null, null, null, null)
        {
            this.session = session;
        }

        public override async Task<User> FindByNameAsync(string userName)
        {
            return await Task.Run(() => new User
            {
                Email = "test@user.com"
            });
        }

        public override async Task<IdentityResult> CreateAsync(User user, string password)
        {
            session.Execute(@"insert into AspNetUsers (Id, UserName, AccessFailedCount, EmailConfirmed, LockoutEnabled, PhoneNumberConfirmed, TwoFactorEnabled) 
                values (@id, @userName, 0, 0, 0, 0, 0)",
                new
                {
                    id = Guid.NewGuid().ToString(),
                    userName = user.UserName
                });

            return await Task.Run(() => IdentityResult.Success);
        }
    }
}
