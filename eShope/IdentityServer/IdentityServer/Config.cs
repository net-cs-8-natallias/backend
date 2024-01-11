using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        { 
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new []
        {
            new ApiScope(name: "catalogitem", displayName: "Catalog API"),
            new ApiScope(name: "basket.item", displayName:"Basket API")
        };

    public static IEnumerable<Client> Clients =>
        new []
        {
            new Client
            {
                ClientId = "client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = {"catalogitem"},
            },
            new Client
            {
                ClientId = "web",
                ClientSecrets = new List<Secret>{new Secret("secret".Sha256())},
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = {"http://localhost:5018/signin-oidc"},
                PostLogoutRedirectUris = {"http://localhost:5018/signout-callback-oidc"},
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                }
            },
            new Client
            {
                ClientId = "basketswaggerui",
                ClientName = "Basket Swagger UI",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,
                ClientSecrets = new List<Secret>{new Secret("secret".Sha256())},
                RedirectUris = { $"http://localhost:5055/swagger/oauth2-redirect.html" },
                PostLogoutRedirectUris = { $"http://localhost:5055/swagger/" },

                AllowedScopes =
                {
                    "mvc", "basket.item"
                }
            },
            new Client
            {
                ClientId = "basket",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                }
            }
        };
}