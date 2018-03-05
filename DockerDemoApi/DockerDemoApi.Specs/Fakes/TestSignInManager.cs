using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DockerDemoApi.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace DockerDemoApi.Specs.Fakes
{
    public class TestSignInManager : SignInManager<User>
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public TestSignInManager(TestUserManager userManager) : base(userManager, new HttpContextAccessor(), new UserClaimsPrincipalFactory<User>(userManager, new OptionsWrapper<IdentityOptions>(new IdentityOptions())), null, null, null)
        {
        }

        public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            return await Task.Run(() => Username == userName && Password == password ? SignInResult.Success : SignInResult.Failed);
        }
    }
}
