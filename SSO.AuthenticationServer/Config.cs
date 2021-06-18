using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.AuthenticationServer
{
    public static class Config
    {
        public static List<IdentityResource> IdentityResources = new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResource()
            {
                 DisplayName="Roles",
                 Name = "roles",
                 UserClaims = new[]{ "role"},
                 Enabled = true
            }
        };
        public static List<Client> Clients = new List<Client>
        {
             new Client
             {
            ClientId = "iclient",
            ClientSecrets = { new Secret("secret".Sha256()) },

            AllowedGrantTypes = GrantTypes.Code,

            // where to redirect to after login
            RedirectUris = { "https://localhost:44314/signin-oidc" },

            // where to redirect to after logout
            PostLogoutRedirectUris = { "https://localhost:44314/signout-callback-oidc" },

            AllowedScopes = new List<string>
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.StandardScopes.Email,
                "roles"
            }
             }
        };
    }
}
