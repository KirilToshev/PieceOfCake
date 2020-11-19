// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace PieceOfCake.IDP
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                        new IdentityResources.OpenId(),
                        new IdentityResources.Profile(),
                        new IdentityResources.Email(),
                        //new IdentityResource("country", new [] { "country" })
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("test", "Piece Of Cake API")
            };

        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
                //new ApiResource("pieceOfCakeApi", "Piece Of Cake API")
                new ApiResource()
                {
                    Name = "pieceOfCakeApi",
                    DisplayName = "Piece Of Cake API",
                    Scopes = new [] { "test" }
                }
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "peiceOfCakeBlazorApp",
                    ClientName = "Piece Of Cake Blazor App",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequirePkce = true,
                    RequireConsent = true,
                    RedirectUris = { "https://localhost:44341/authentication/login-callback" },
                    PostLogoutRedirectUris = { "https://localhost:44341/authentication/logout-callback" },
                    AllowedScopes = { "openid", "profile", "email", "test" },
                    AllowedCorsOrigins = { "https://localhost:44341" }
                }
            };
    }
}