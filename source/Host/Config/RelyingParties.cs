﻿using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel.Constants;
using IdentityServer3.WsFederation.Models;

namespace Host.Config
{
    internal static class RelyingParties
    {
        public static IEnumerable<RelyingParty> Get()
        {
            return new List<RelyingParty>
            {
                new RelyingParty
                {
                    Realm = "urn:testrp",
                    Name = "Test RP",
                    Enabled = true,
                    ReplyUrl = "https://web.local/idsrvrp/",
                    TokenType = TokenTypes.Saml2TokenProfile11,
                    TokenLifeTime = 60,
                    ClaimMappings = new Dictionary<string, string>
                    {
                        {"sub", ClaimTypes.NameIdentifier},
                        {"given_name", ClaimTypes.Name},
                        {"email", ClaimTypes.Email}
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "https://web.local/idsrvrp/"
                    }
                },
                new RelyingParty
                {
                    Realm = "urn:owinrp",
                    Enabled = true,
                    ReplyUrl = "http://localhost:10313/",
                    TokenType = TokenTypes.JsonWebToken,
                    TokenLifeTime = 60,
                    ClaimMappings = new Dictionary<string, string>
                    {
                        {"sub", ClaimTypes.NameIdentifier},
                        {"name", ClaimTypes.Name},
                        {"given_name", ClaimTypes.GivenName},
                        {"email", ClaimTypes.Email}
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "http://localhost:10313/"
                    }
                }
            };
        }
    }
}