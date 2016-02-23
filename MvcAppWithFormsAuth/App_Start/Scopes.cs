using System.Collections.Generic;
using IdentityServer3.Core.Models;

namespace MvcAppWithFormsAuth
{
    static class Scopes
    {
        public static List<Scope> Get()
        {
            return new List<Scope>
            {
                StandardScopes.OpenId,
                StandardScopes.OfflineAccess,
                new Scope
                {
                    Name = "reporting_api"
                }
            };
        }
    }
}