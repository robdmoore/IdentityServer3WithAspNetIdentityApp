using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services.InMemory;
using Owin;

namespace MvcAppWithFormsAuth
{
    partial class Startup
    {
        public void ConfigureIdentityServer(IAppBuilder app)
        {
            var options = new IdentityServerOptions
            {
                Factory = new IdentityServerServiceFactory()
                            .UseInMemoryClients(Clients.Get())
                            .UseInMemoryScopes(Scopes.Get())
                            .UseInMemoryUsers(new List<InMemoryUser>()),
                SigningCertificate = LoadCertificate(),
                RequireSsl = false,
                EnableWelcomePage = false
            };

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