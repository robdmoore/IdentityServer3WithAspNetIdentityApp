﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.Default;
using IdentityServer3.Core.Services.InMemory;
using Owin;

namespace MvcAppWithFormsAuth
{
    partial class Startup
    {
        public void ConfigureIdentityServer(IAppBuilder app)
        {
            // https://github.com/IdentityServer/IdentityServer3/issues/2332

            var options = new IdentityServerOptions
            {
                Factory = new IdentityServerServiceFactory()
                            .UseInMemoryClients(Clients.Get())
                            .UseInMemoryScopes(Scopes.Get()),
                SigningCertificate = LoadCertificate(),
                RequireSsl = false,
                EnableWelcomePage = false,
                SiteName = "Authentication service"
            };

            // The next line hooks up ASP.NET identity user store to Identity Server so the Identity Server login page can be used in place of the ASP.NET identity one
            //options.Factory.UserService = new Registration<IUserService>(resolver => new AspNetIdentityUserService<ApplicationUser, string>(HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()));
            options.Factory.UserService = new Registration<IUserService>(_ => new CookieAuthUserService(Users.Get()));

            app.Map("/identity", idApp => idApp.UseIdentityServer(options));
        }

        X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2(
                string.Format(@"{0}\bin\App_Start\idsrv3test.pfx", AppDomain.CurrentDomain.BaseDirectory),
                "idsrv3test");
        }
    }
}