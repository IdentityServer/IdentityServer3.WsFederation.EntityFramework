using System.Collections.Generic;
using System.Data.Entity;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using WsFederationPlugin.EntityFramework.Entities;
using Xunit;

namespace WsFederationPlugin.EntityFramework.IntegrationTests
{
    public class RelyingPartyConfigurationDbContextTests
    {
        private const string ConfigConnectionStringName = "Config";

        public RelyingPartyConfigurationDbContextTests()
        {
            Database.SetInitializer(
                new DropCreateDatabaseAlways<RelyingPartyConfigurationDbContext>());
        }

        [Fact]
        public void CanAddRelyingPartyWithClaimMappings()
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

            using (var context = new RelyingPartyConfigurationDbContext(ConfigConnectionStringName))
            {
                context.RelyingParties.Add(relyingParty);
                context.SaveChanges();
            }

            RelyingParty persistedRelyingParty;
            using (var context = new RelyingPartyConfigurationDbContext(ConfigConnectionStringName))
            {
                persistedRelyingParty =
                    context.RelyingParties.Include(x => x.ClaimMappings)
                        .FirstOrDefault(x => x.Realm == relyingParty.Realm);
            }

            Assert.NotNull(persistedRelyingParty);
            Assert.True(relyingParty.Realm == persistedRelyingParty.Realm);
            Assert.True(persistedRelyingParty.ClaimMappings.Any());
            Assert.True(relyingParty.ClaimMappings.Count == persistedRelyingParty.ClaimMappings.Count);
            Assert.True(relyingParty.DefaultClaimTypeMappingPrefix ==
                        persistedRelyingParty.DefaultClaimTypeMappingPrefix);
            Assert.True(relyingParty.DigestAlgorithm == persistedRelyingParty.DigestAlgorithm);
            Assert.True(relyingParty.Enabled == persistedRelyingParty.Enabled);
            Assert.True(relyingParty.Id == persistedRelyingParty.Id);
            Assert.True(relyingParty.IncludeAllClaimsForUser == persistedRelyingParty.IncludeAllClaimsForUser);
            Assert.True(relyingParty.Name == persistedRelyingParty.Name);
            Assert.True(relyingParty.ReplyUrl == persistedRelyingParty.ReplyUrl);
            Assert.True(relyingParty.SamlNameIdentifierFormat == persistedRelyingParty.SamlNameIdentifierFormat);
            Assert.True(relyingParty.SignatureAlgorithm == persistedRelyingParty.SignatureAlgorithm);
            Assert.True(relyingParty.TokenLifeTime == persistedRelyingParty.TokenLifeTime);
            Assert.True(relyingParty.TokenType == persistedRelyingParty.TokenType);
        }
    }
}