using DockerDemoApi.Common;
using DockerDemoApi.Specs.Steps;
using Xbehave;

namespace DockerDemoApi.Specs
{
    public class LoginFeature : BaseFeature
    {
        readonly AccountSteps accountSteps;

        public LoginFeature()
        {
            accountSteps = ApplicationContext.Resolve<AccountSteps>();
        }

        [Scenario]
        public void AUserCanLogIn()
        {
            "Given I have the following login credentials"
                .x(() => accountSteps.StoreLoginCredentials(new
                {
                    Username = "MOzil",
                    Password = "1Strong"
                }));

            "When I log in"
                .x(() => accountSteps.LogIn(new
                {
                    Username = "MOzil",
                    Password = "1Strong"
                }));

            "Then I am succesfully logged in"
                .x(() => accountSteps.AssertUserIsLoggedIn());
        }

        [Scenario]
        [Example(null, "1Strong")]
        [Example("MOzil", null)]
        [Example(null, null)]
        [Example("MOzil", "Wrongpassword")]
        public void AUserCannotLogInWhenProvidingInvalidCredentials(string username, string password)
        {
            "Given I have the following login credentials"
                .x(() => accountSteps.StoreLoginCredentials(new
                {
                    Username = "MOzil",
                    Password = "1Strong"
                }));

            "When I log in"
                .x(() => accountSteps.LogIn(new
                {
                    Username = username,
                    Password = password
                }));

            "Then I am unable to logged in"
                .x(() => accountSteps.AssertUserIsNotLoggedIn());
        }
    }
}
