using System.Collections.Generic;
using IdentityServer3.Core.Services.InMemory;

namespace MvcAppWithFormsAuth
{
    static class Users
    {
        public static List<InMemoryUser> Get()
        {
            return new List<InMemoryUser>
            {
                new InMemoryUser
                {
                    Username = "username",
                    Password = "password",
                    Subject = "user123"
                }
            };
        }
    }
}