using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DockerDemoApi.Controllers;
using DockerDemoApi.Domain;
using DockerDemoApi.Models;
using DockerDemoApi.Orm;
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
        readonly ISession session;

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

        public void StoreAccount(dynamic accountDetails)
        {
            string username = accountDetails.Username;
            session.Execute(@"
                INSERT INTO AspNetUsers (Id, UserName, AccessFailedCount, EmailConfirmed, LockoutEnabled, PhoneNumberConfirmed, TwoFactorEnabled) 
                Values (@id, @username, 0, 0, 0, 0, 0)",
                new
                {
                    id = Guid.NewGuid().ToString(),
                    username
                });
        }

        public async Task LogIn(dynamic logInDetails)
        {
            loginResult = await controller.SignIn(new CredentialsModel
            {
                Username = logInDetails.Username,
                Password = logInDetails.Password
            });
        }

        public async Task RegisterAccount(dynamic registerDetails)
        {
            await controller.Register(new RegisterModel
            {
                Username = registerDetails.Username,
                Password = registerDetails.Password,
                ConfirmPassword = registerDetails.ConfirmPassword,
                AgreeToTermsAndConditions = registerDetails.AgreeToTermsAndConditions
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

        public void AssertAccountExists(string username)
        {
            var result = session.Query<User>(@"select * from AspNetUsers where Username = @username", new { username });
            result.Should().HaveCount(1);
        }
    }
}
