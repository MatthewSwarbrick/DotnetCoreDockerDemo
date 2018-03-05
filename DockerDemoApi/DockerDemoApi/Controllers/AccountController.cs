using System.Collections.Generic;
using System.Threading.Tasks;
using DockerDemoApi.Domain;
using DockerDemoApi.Helpers;
using DockerDemoApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DockerDemoApi.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        readonly UserManager<User> userManager;
        readonly SignInManager<User> signInManager;
        readonly JWTSettings options;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IOptions<JWTSettings> optionsAccessor)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            options = optionsAccessor.Value;
        }

        [HttpPost("sign-in")]
        public async Task<JsonResult> SignIn(CredentialsModel Credentials)
        {
            var result = await signInManager.PasswordSignInAsync(Credentials.Username, Credentials.Password, false, false);
            if (result.Succeeded)
            {
                var user = await userManager.FindByNameAsync(Credentials.Username);
                return new JsonResult(new Dictionary<string, string>
                {
                    { "access_token", AuthenticationHelper.GetAccessToken(user.Email, options) },
                    { "id_token", AuthenticationHelper.GetIdToken(user, options) }
                });
            }

            return new JsonResult("Unable to sign in") { StatusCode = 400 };
        }

        [HttpPost("register")]
        public async Task Register(RegisterModel model)
        {
            var user = new User
            {
                UserName = model.Username
            };

            await userManager.CreateAsync(user, model.Password);
        }
    }
}
