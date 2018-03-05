using System.Collections.Generic;
using System.Threading.Tasks;
using DockerDemoApi.Controllers;
using DockerDemoApi.Domain;
using DockerDemoApi.Models;
using DockerDemoApi.Specs.Fakes;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DockerDemoApi.Specs.Steps
{
    public class AccountSteps
    {
        JsonResult loginResult;

        readonly AccountController controller;
        readonly TestSignInManager signInManager;

        public AccountSteps(
            AccountController controller,
            SignInManager<User> signInManager)
        {
            this.controller = controller;
            this.signInManager = signInManager as TestSignInManager;
        }

        public void StoreLoginCredentials(dynamic loginCredentials)
        {
            signInManager.Username = loginCredentials.Username;
            signInManager.Password = loginCredentials.Password;
        }

        public async Task LogIn(dynamic logInDetails)
        {
            loginResult = await controller.SignIn(new CredentialsModel
            {
                Username = logInDetails.Username,
                Password = logInDetails.Password
            });
        }

        public void AssertUserIsLoggedIn()
        {
            var tokens = loginResult.Value as Dictionary<string, string>;
            tokens["access_token"].Should().NotBeNullOrWhiteSpace();
            tokens["id_token"].Should().NotBeNullOrWhiteSpace();
        }

        public void AssertUserIsNotLoggedIn()
        {
            loginResult.StatusCode.Should().Be(400);
            loginResult.Value.Should().Be("Unable to sign in");
        }
    }
}
