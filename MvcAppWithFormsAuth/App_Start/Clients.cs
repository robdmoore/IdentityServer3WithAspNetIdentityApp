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
           // no human involved
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
            }
        };
        }
    }
}