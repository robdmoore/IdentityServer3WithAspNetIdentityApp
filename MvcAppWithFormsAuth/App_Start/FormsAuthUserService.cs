using System;
using System.IO;
using System.IO.Compression;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.Default;

namespace MvcAppWithFormsAuth
{
    public class FormsAuthUserService : UserServiceBase
    {
        public override Task PreAuthenticateAsync(PreAuthenticationContext context)
        {
            var request = HttpContext.Current.Request;
            var cookie = request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null)
            {

                var ticket = cookie.Value;
                ticket = ticket.Replace('-', '+').Replace('_', '/');

                var padding = 3 - ((ticket.Length + 3) % 4);
                if (padding != 0)
                    ticket = ticket + new string('=', padding);
                var bytes = Convert.FromBase64String(ticket);
                bytes = System.Web.Security.MachineKey.Unprotect(bytes,
                    "Microsoft.Owin.Security.Cookies.CookieAuthenticationMiddleware",
                    "ApplicationCookie",
                    "v1");

                using (var memory = new MemoryStream(bytes))
                {
                    using (var compression = new GZipStream(memory, CompressionMode.Decompress))
                    {
                        using (var reader = new BinaryReader(compression))
                        {
                            reader.ReadInt32(); // Ignoring version here
                            string authenticationType = reader.ReadString();
                            reader.ReadString(); // Ignoring the default name claim type
                            reader.ReadString(); // Ignoring the default role claim type

                            int count = reader.ReadInt32(); // count of claims in the ticket

                            var claims = new Claim[count];
                            for (int index = 0; index != count; ++index)
                            {
                                string type = reader.ReadString();
                                type = type == "\0" ? ClaimTypes.Name : type;

                                string value = reader.ReadString();

                                string valueType = reader.ReadString();
                                valueType = valueType == "\0" ?
                                                "http://www.w3.org/2001/XMLSchema#string" : valueType;

                                string issuer = reader.ReadString();
                                issuer = issuer == "\0" ? "LOCAL AUTHORITY" : issuer;

                                string originalIssuer = reader.ReadString();
                                originalIssuer = originalIssuer == "\0" ? issuer : originalIssuer;

                                claims[index] = new Claim(type, value, valueType, issuer, originalIssuer);
                            }

                            var identity = new ClaimsIdentity(claims, authenticationType,
                                                                  ClaimTypes.Name, ClaimTypes.Role);

                            var principal = new ClaimsPrincipal(identity);
                            System.Threading.Thread.CurrentPrincipal = principal;
                            if (HttpContext.Current != null)
                                HttpContext.Current.User = principal;
                        }
                    }
                }
            }


            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                context.AuthenticateResult = new AuthenticateResult(
                    HttpContext.Current.User.Identity.Name,
                    HttpContext.Current.User.Identity.Name,
                    null,
                    "idsrv",
                    "formsauth");
            }
            else
            {
                HttpContext.Current.Response.Redirect("/Account/Login?ReturnUrl=" + HttpUtility.UrlEncode(HttpContext.Current.Request.Url.ToString()));
            }

            return base.PreAuthenticateAsync(context);
        }
    }
}