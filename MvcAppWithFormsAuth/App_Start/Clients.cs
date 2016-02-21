using System.Collections.Generic;
using IdentityServer3.Core.Models;

namespace MvcAppWithFormsAuth
{
    static class Clients
    {
        public static List<Client> Get()
        {
            return new List<Client>
        {
            new Client
            {
                ClientName = "Reporting Site",
                ClientId = "reporting",
                Enabled = true,
                AccessTokenType = AccessTokenType.Jwt,

                Flow = Flows.ClientCredentials,

                ClientSecrets = new List<Secret>
                {
                    new Secret("F621F470-9731-4A25-80EF-67A6F7C5F4B8".Sha256())
                },

                AllowedScopes = new List<string>
                {
                    "reporting_api"
                },

                AllowedCorsOrigins = new List<string>
                {
                    "http://anotherapp.localtest.me"
                }
            },
            new Client
            {
                ClientName = "Reporting Site via Login",
                ClientId = "reporting_with_login",
                Enabled = true,
                AccessTokenType = AccessTokenType.Jwt,
                Flow = Flows.Implicit,
                AllowedScopes = new List<string>
                {
                    "reporting_api",
                    StandardScopes.OpenId.Name
                },
                AllowedCorsOrigins = new List<string>
                {
                    "http://anotherapp.localtest.me"
                },
                RedirectUris = new List<string>
                {
                    "http://anotherapp.localtest.me/"
                },
                RequireConsent = false
            }
        };
        }
    }
}