using DockerDemoApi.Common;
using DockerDemoApi.Common.Exceptions;
using DockerDemoApi.Specs.Helpers;
using DockerDemoApi.Specs.Steps;
using Xbehave;

namespace DockerDemoApi.Specs
{
    public class RegisterAccountFeature : BaseFeature
    {
        readonly AccountSteps accountSteps;
        readonly SharedSteps sharedSteps;
        readonly SharedStepsContext sharedStepsContext;

        public RegisterAccountFeature()
        {
            accountSteps = ApplicationContext.Resolve<AccountSteps>();
            sharedSteps = ApplicationContext.Resolve<SharedSteps>();
            sharedStepsContext = ApplicationContext.Resolve<SharedStepsContext>();
        }

        [Scenario]
        public void ICanRegisterAnAccount()
        {
            "When I register an account with the following details"
            .x(async () =>
            {
                await accountSteps.RegisterAccount(new
                {
                    Username = "MOzil",
                    Password = "1StrongPassword!",
                    ConfirmPassword = "1StrongPassword!",
                    AgreeToTermsAndConditions = true
                });
            });

            "Then the following account is created"
            .x(() =>
            {
                accountSteps.AssertAccountExists(username: "MOzil");
            });
        }

        [Scenario]
        [Example("MOZIL")]
        [Example("mozil")]
        public void IAmUnableToRegisterAnAccountWithUsernameThatAlreadyExists(string username)
        {
            "Given I have the following account stored"
            .x(() =>
            {
                accountSteps.StoreAccount(new
                {
                    Username = "MOzil"
                });
            });

            "When I register an account with the following details"
            .x(() =>
            {
                sharedStepsContext.CaughtException = Catch.ExceptionAsync(async () =>
                    await accountSteps.RegisterAccount(new
                    {
                        Username = username,
                        Password = "1StrongPassword!",
                        ConfirmPassword = "1StrongPassword!",
                        AgreeToTermsAndConditions = true
                    }));
            });

            "Then an error occurs"
            .x(() =>
            {
                sharedSteps.AssertExceptionThrown<RegisterFailedException>("Account with this username already exists");
            });

            "And no new user is registered"
            .x(() =>
            {
                accountSteps.AssertAccountExists(username: "MOzil");
            });
        }
    }
}
