using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using IdentityServer3.WsFederation.EntityFramework;
using IdentityServer3.WsFederation.EntityFramework.Entities;
using IdentityServer3.WsFederation.Models;
using Xunit;
using RelyingParty = IdentityServer3.WsFederation.EntityFramework.Entities.RelyingParty;

namespace WsFederationPlugin.EntityFramework.IntegrationTests
{
    public class AutomapperTests
    {
        [Fact]
        public void AutomapperConfigurationIsValidToModel()
        {
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

            // TODO: EncryptingCertificate
            Assert.True(model.EncryptingCertificate == null);

            EntitiesMap.Mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Fact]
        public void AutomapperConfigurationIsValidToEntity()
        {
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

            // TODO: EncryptingCertificate
            Assert.True(entity.EncryptingCertificate == null);

            ModelsMap.Mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}