using System;
using DockerDemoApi.Common;
using DockerDemoApi.Domain;
using DockerDemoApi.Orm;
using DockerDemoApi.Specs.Fakes;
using DockerDemoApi.Specs.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace DockerDemoApi.Specs
{
    public class BaseFeature : IDisposable
    {
        readonly TestOrmSession testOrmSession;

        public BaseFeature()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            container.RegisterSingleton(new SharedStepsContext());

            // register orm session
            testOrmSession = new TestOrmSession();
            container.RegisterSingleton<ISession>(testOrmSession);

            //register fakes
            var testUserManager = new TestUserManager(testOrmSession);
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

            testOrmSession.Begin();
            AsyncScopedLifestyle.BeginScope(container);

            ApplicationContext.SetContainer(container);
        }

        public void Dispose()
        {
            testOrmSession.Rollback();
            Lifestyle.Scoped.GetCurrentScope(ApplicationContext.GetContainer())?.Dispose();
        }
    }
}
