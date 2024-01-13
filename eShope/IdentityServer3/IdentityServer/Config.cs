using IdentityServer4.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[]
            {
                // new ApiResource("alevelwebsite.com")
                new ApiResource("web")
                {
                    Scopes = new List<Scope>
                    {
                        new Scope("mvc")
                    },
                },
                new ApiResource("catalog")
                {
                    Scopes = new List<Scope>
                    {
                        new Scope("catalog.catalogitem"),
                    },
                },
                new ApiResource("basket")
                {
                    Scopes = new List<Scope>
                    {
                        new Scope("basket.basketitem"),
                    },
                }
            };
        }

        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            return new[]
            {
                new Client
                {
                    ClientId = "mvc_pkce",
                    ClientName = "MVC PKCE Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets = {new Secret("secret".Sha256())},
                    RedirectUris = { $"{configuration["MvcUrl"]}/signin-oidc"},
                    AllowedScopes = {"openid", "profile", "mvc"},
                    RequirePkce = true,
                    RequireConsent = false
                },
                new Client
                {
                    ClientId = "catalog",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    }
                },
                new Client
                {
                    ClientId = "catalogswaggerui",
                    ClientName = "Catalog Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{configuration["CatalogApi"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{configuration["CatalogApi"]}/swagger/" },

                    AllowedScopes =
                    {
                        "mvc", "catalog.catalogitem"
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
                },
                new Client
                {
                    ClientId = "basketswaggerui",
                    ClientName = "Basket Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{configuration["BasketApi"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{configuration["BasketApi"]}/swagger/" },

                    AllowedScopes =
                    {
                        "mvc", "basket.basketitem"
                    }
                },
            };
        }
    }
}
//     public static class Config
//     {
//         public static IEnumerable<IdentityResource> GetIdentityResources()
//         {
//             return new IdentityResource[]
//             {
//                 new IdentityResources.OpenId(),
//                 new IdentityResources.Profile()
//             };
//         }
//
//         public static IEnumerable<ApiResource> GetApis()
//         {
//             return new ApiResource[]
//             {
//                 new ApiResource("web")
//                 {
//                     Scopes = new List<Scope>
//                     {
//                         new Scope("mvc")
//                     },
//                 },
//                 new ApiResource("catalog")
//                 {
//                     Scopes = new List<Scope>
//                     {
//                         new Scope("catalogitem"),
//                     },
//                 },
//                 new ApiResource("basket")
//                 {
//                     Scopes = new List<Scope>
//                     {
//                         new Scope("basketitem"),
//                     },
//                 }
//             };
//         }
//
//         public static IEnumerable<Client> GetClients(IConfiguration configuration)
//         {
//             return new[]
//             {
//                 new Client
//                 {
//                     ClientId = "mvc_pkce",
//                     ClientName = "MVC PKCE Client",
//                     AllowedGrantTypes = GrantTypes.Code,
//                     ClientSecrets = {new Secret("secret".Sha256())},
//                     RedirectUris = { $"{configuration["MvcUrl"]}/signin-oidc"},
//                     AllowedScopes = {"openid", "profile", "mvc"},
//                     RequirePkce = true,
//                     RequireConsent = false
//                 },
//                 new Client
//                 {
//                     ClientId = "catalog",
//                     AllowedGrantTypes = GrantTypes.ClientCredentials,
//                     ClientSecrets =
//                     {
//                         new Secret("secret".Sha256())
//                     }
//                 },
//                 new Client
//                 {
//                     ClientId = "catalogswaggerui",
//                     ClientName = "Catalog Swagger UI",
//                     AllowedGrantTypes = GrantTypes.Implicit,
//                     AllowAccessTokensViaBrowser = true,
//
//                     RedirectUris = { $"{configuration["CatalogApi"]}/swagger/oauth2-redirect.html" },
//                     PostLogoutRedirectUris = { $"{configuration["CatalogApi"]}/swagger/" },
//
//                     AllowedScopes =
//                     {
//                         "mvc", "catalogitem"
//                     }
//                 },
//                 new Client
//                 {
//                     ClientId = "basket",
//                     AllowedGrantTypes = GrantTypes.ClientCredentials,
//                     ClientSecrets =
//                     {
//                         new Secret("secret".Sha256())
//                     }
//                 },
//                 new Client
//                 {
//                     ClientId = "basketswaggerui",
//                     ClientName = "Basket Swagger UI",
//                     AllowedGrantTypes = GrantTypes.Implicit,
//                     AllowAccessTokensViaBrowser = true,
//
//                     RedirectUris = { $"{configuration["BasketApi"]}/swagger/oauth2-redirect.html" },
//                     PostLogoutRedirectUris = { $"{configuration["BasketApi"]}/swagger/" },
//
//                     AllowedScopes =
//                     {
//                         "mvc", "basket.basketitem"
//                     }
//                 },
//             };
//         }
//     }
