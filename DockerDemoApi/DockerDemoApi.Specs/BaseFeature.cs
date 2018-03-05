using System;
using DockerDemoApi.Common;
using DockerDemoApi.Domain;
using DockerDemoApi.Specs.Fakes;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace DockerDemoApi.Specs
{
    public class BaseFeature : IDisposable
    {

        public BaseFeature()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            //register fakes
            var testUserManager = new TestUserManager();
            var testSignInManager = new TestSignInManager(testUserManager);
            container.Register<SignInManager<User>>(() => testSignInManager, Lifestyle.Scoped);
            container.Register<UserManager<User>>(() => testUserManager, Lifestyle.Scoped);

            var jwtSettings = new OptionsWrapper<JWTSettings>(new JWTSettings
            {
                Audience = "Test",
                Issuer = "Test",
                SecretKey = "Test"
            });
            container.Register<IOptions<JWTSettings>>(() => jwtSettings, Lifestyle.Scoped);

            container.Verify();
            
            AsyncScopedLifestyle.BeginScope(container);

            ApplicationContext.SetContainer(container);
        }

        public void Dispose()
        {
            Lifestyle.Scoped.GetCurrentScope(ApplicationContext.GetContainer())?.Dispose();
        }
    }
}
