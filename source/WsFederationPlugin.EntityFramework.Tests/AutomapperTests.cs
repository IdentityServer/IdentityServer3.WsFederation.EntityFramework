using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using IdentityServer3.WsFederation.EntityFramework.Entities;
using IdentityServer3.WsFederation.Models;
using WsFederationPlugin.EntityFramework.Tests.Certificates;
using Xunit;
using RelyingParty = IdentityServer3.WsFederation.EntityFramework.Entities.RelyingParty;

namespace WsFederationPlugin.EntityFramework.Tests
{
    public class AutomapperTests
    {
        [Fact]
        public void AutomapperConfigurationIsValidToModel()
        {
            var encryptingCertificate = Cert.LoadEncryptingCertificate();

            var relyingParty = new RelyingParty
            {
                Realm = "urn:identityserver",
                Name = "Test Relying Party",
                Enabled = true,
                ReplyUrl = "https://www.google.com/",
                TokenType = "urn:oasis:names:tc:SAML:1.0:assertion",
                TokenLifeTime = 1000,
                IncludeAllClaimsForUser = false,
                DefaultClaimTypeMappingPrefix = "https://schema.org/",
                SamlNameIdentifierFormat = "urn:oasis:names:tc:SAML:1.1:nameid-format:emailAddress",
                SignatureAlgorithm = SecurityAlgorithms.RsaSha256Signature,
                DigestAlgorithm = SecurityAlgorithms.Sha256Digest,
                ClaimMappings = new List<ClaimMap>
                {
                    new ClaimMap {InboundClaim = "name", OutboundClaim = ClaimTypes.Name},
                    new ClaimMap {InboundClaim = "email", OutboundClaim = ClaimTypes.Email}
                },
                EncryptingCertificate = encryptingCertificate.RawData,
                PostLogoutRedirectUris = new List<RelyingPartyPostLogoutUri>
                {
                    new RelyingPartyPostLogoutUri {Uri = "https://www.google.com/post"},
                    new RelyingPartyPostLogoutUri {Uri = "https://www.google.com/post2"}
                }
            };

            var model = relyingParty.ToModel();

            Assert.NotNull(model);
            Assert.True(model.Realm == relyingParty.Realm);
            Assert.True(model.Name == relyingParty.Name);
            Assert.True(model.Enabled == relyingParty.Enabled);
            Assert.True(model.ReplyUrl == relyingParty.ReplyUrl);
            Assert.True(model.TokenType == relyingParty.TokenType);
            Assert.True(model.TokenLifeTime == relyingParty.TokenLifeTime);
            Assert.True(model.IncludeAllClaimsForUser == relyingParty.IncludeAllClaimsForUser);
            Assert.True(model.DefaultClaimTypeMappingPrefix == relyingParty.DefaultClaimTypeMappingPrefix);
            Assert.True(model.SamlNameIdentifierFormat == relyingParty.SamlNameIdentifierFormat);
            Assert.True(model.DigestAlgorithm == relyingParty.DigestAlgorithm);

            Assert.NotNull(model.ClaimMappings);
            Assert.True(model.ClaimMappings.Any());
            Assert.True(model.ClaimMappings.First(x => x.Key == "name").Value == ClaimTypes.Name);

            Assert.NotNull(model.EncryptingCertificate);
            Assert.True(model.EncryptingCertificate.Subject == encryptingCertificate.Subject);
            Assert.True(model.EncryptingCertificate.Issuer == encryptingCertificate.Issuer);
            Assert.True(model.EncryptingCertificate.Thumbprint == encryptingCertificate.Thumbprint);
            
            Assert.NotNull(model.PostLogoutRedirectUris);
            Assert.NotEmpty(model.PostLogoutRedirectUris);

            EntitiesMap.Mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Fact]
        public void AutomapperConfigurationIsValidToEntity()
        {
            var encryptingCertificate = Cert.LoadEncryptingCertificate();

            var relyingParty = new IdentityServer3.WsFederation.Models.RelyingParty
            {
                Realm = "urn:identityserver",
                Name = "Test Relying Party",
                Enabled = true,
                ReplyUrl = "https://www.google.com/",
                TokenType = "urn:oasis:names:tc:SAML:1.0:assertion",
                TokenLifeTime = 1000,
                IncludeAllClaimsForUser = false,
                DefaultClaimTypeMappingPrefix = "https://schema.org/",
                SamlNameIdentifierFormat = "urn:oasis:names:tc:SAML:1.1:nameid-format:emailAddress",
                SignatureAlgorithm = SecurityAlgorithms.RsaSha256Signature,
                DigestAlgorithm = SecurityAlgorithms.Sha256Digest,
                ClaimMappings = new Dictionary<string, string>
                {
                    {"name", ClaimTypes.Name },
                    {"email", ClaimTypes.Email }
                },
                EncryptingCertificate = encryptingCertificate,
                PostLogoutRedirectUris = new List<string>
                {
                    "https://www.google.com/post",
                    "https://www.google.com/post2"
                }
            };

            var entity = relyingParty.ToEntity();

            Assert.NotNull(entity);
            Assert.True(entity.Realm == relyingParty.Realm);
            Assert.True(entity.Name == relyingParty.Name);
            Assert.True(entity.Enabled == relyingParty.Enabled);
            Assert.True(entity.ReplyUrl == relyingParty.ReplyUrl);
            Assert.True(entity.TokenType == relyingParty.TokenType);
            Assert.True(entity.TokenLifeTime == relyingParty.TokenLifeTime);
            Assert.True(entity.IncludeAllClaimsForUser == relyingParty.IncludeAllClaimsForUser);
            Assert.True(entity.DefaultClaimTypeMappingPrefix == relyingParty.DefaultClaimTypeMappingPrefix);
            Assert.True(entity.SamlNameIdentifierFormat == relyingParty.SamlNameIdentifierFormat);
            Assert.True(entity.DigestAlgorithm == relyingParty.DigestAlgorithm);

            Assert.NotNull(entity.ClaimMappings);
            Assert.True(entity.ClaimMappings.Any());
            Assert.True(entity.ClaimMappings.First(x => x.InboundClaim == "name").OutboundClaim == ClaimTypes.Name);
            
            Assert.NotNull(entity.EncryptingCertificate);
            var loadedCertificate = new X509Certificate2(entity.EncryptingCertificate);

            Assert.True(loadedCertificate.Subject == encryptingCertificate.Subject);
            Assert.True(loadedCertificate.Issuer == encryptingCertificate.Issuer);
            Assert.True(loadedCertificate.Thumbprint == encryptingCertificate.Thumbprint);

            Assert.NotNull(entity.PostLogoutRedirectUris);
            Assert.NotEmpty(entity.PostLogoutRedirectUris);

            ModelsMap.Mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Fact]
        public void ToEntity_WhenEncryptionCertificateIsNull_ExpectNullEncryptionCertificate()
        {
            var model = new IdentityServer3.WsFederation.Models.RelyingParty();
            var entity = model.ToEntity();

            Assert.Null(entity.EncryptingCertificate);
        }
    }
}