using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.InMemory;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataProtection;
using AuthenticateResult = IdentityServer3.Core.Models.AuthenticateResult;

namespace MvcAppWithFormsAuth
{
    public class CookieAuthUserService : InMemoryUserService
    {

        // This ctor isn't needed for CookieAuthUserService (the base class should be UserServiceBase), but it allows for my demo to have password flow work against in-memory users too
        public CookieAuthUserService(List<InMemoryUser> users)
            : base(users)
        {
        }

        public override Task PreAuthenticateAsync(PreAuthenticationContext context)
        {
            var cookie = HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null)
            {
                var secureDataFormat = new TicketDataFormat(new MachineKeyProtector());
                var ticket = secureDataFormat.Unprotect(cookie.Value);

                context.AuthenticateResult = new AuthenticateResult(
                    ticket.Identity.Name,
                    ticket.Identity.Name,
                    null,
                    "idsrv",
                    "cookieauth");
            }
            else
            {
                HttpContext.Current.Response.Redirect("/Account/Login?ReturnUrl=" + HttpUtility.UrlEncode(HttpContext.Current.Request.Url.ToString()));
            }

            return base.PreAuthenticateAsync(context);
        }
    }

    public class MachineKeyProtector : IDataProtector
    {
        private readonly string[] _purpose =
        {
            "Microsoft.Owin.Security.Cookies.CookieAuthenticationMiddleware",
            "ApplicationCookie",
            "v1"
        };

        public byte[] Protect(byte[] userData)
        {
            throw new NotImplementedException();
        }

        public byte[] Unprotect(byte[] protectedData)
        {
            return MachineKey.Unprotect(protectedData, _purpose);
        }
    }
}