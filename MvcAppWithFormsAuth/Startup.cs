using System;
using Microsoft.Owin;
using Owin;
using Serilog;

[assembly: OwinStartupAttribute(typeof(MvcAppWithFormsAuth.Startup))]
namespace MvcAppWithFormsAuth
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.ColoredConsole()
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();
            Log.Information("Hello, {Name}!", Environment.UserName);

            ConfigureAuth(app);
            ConfigureIdentityServer(app);
        }
    }
}
