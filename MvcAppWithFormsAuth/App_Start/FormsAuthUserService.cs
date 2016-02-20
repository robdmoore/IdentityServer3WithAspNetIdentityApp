using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using IdentityServer3.Core.Extensions;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.Default;

namespace MvcAppWithFormsAuth
{
    public class FormsAuthUserService : UserServiceBase
    {
        public override Task PreAuthenticateAsync(PreAuthenticationContext context)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                context.AuthenticateResult = new AuthenticateResult(
                    HttpContext.Current.User.Identity.GetSubjectId(),
                    HttpContext.Current.User.Identity.GetName(),
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