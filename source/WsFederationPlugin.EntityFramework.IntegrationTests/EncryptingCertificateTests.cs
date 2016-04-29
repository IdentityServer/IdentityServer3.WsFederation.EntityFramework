using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using IdentityServer3.WsFederation.EntityFramework;
using IdentityServer3.WsFederation.EntityFramework.Entities;
using WsFederationPlugin.EntityFramework.IntegrationTests.Certificates;
using Xunit;
using Saml2SecurityTokenHandler = Microsoft.IdentityModel.Tokens.Saml2SecurityTokenHandler;

namespace WsFederationPlugin.EntityFramework.IntegrationTests
{
    public class EncryptingCertificateTests
    {
        private const string ConfigConnectionStringName = "Config";

        public EncryptingCertificateTests()
        {
            Database.SetInitializer(
                new DropCreateDatabaseAlways<RelyingPartyConfigurationDbContext>());
        }

        [Fact]
        public void CanAddRelyingPartyWithClaimMappings()
        {
            var encryptingCertificate = Cert.LoadEncryptingCertificate();

            var relyingParty = new RelyingParty
            {
                Realm = "urn:identityserver:encrypt",
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
                EncryptingCertificate = encryptingCertificate.RawData
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
            
            Assert.NotNull(relyingParty.EncryptingCertificate);
            var readEncryptionCertificate = new X509Certificate2(relyingParty.EncryptingCertificate);

            Assert.True(readEncryptionCertificate.SubjectName.Name == encryptingCertificate.SubjectName.Name);
            Assert.True(readEncryptionCertificate.IssuerName.Name == encryptingCertificate.IssuerName.Name);
            Assert.True(readEncryptionCertificate.Thumbprint == encryptingCertificate.Thumbprint);

            // assert token encrypted using saved cert can be decrypted
            var testClaim = new Claim(ClaimTypes.NameIdentifier, "69307727-D2F7-4ED3-A7DF-08EEE8F8C624");
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                AppliesToAddress = relyingParty.Realm,
                Lifetime = new Lifetime(DateTime.UtcNow, DateTime.UtcNow.AddMinutes(relyingParty.TokenLifeTime)),
                ReplyToAddress = relyingParty.ReplyUrl,
                SigningCredentials = new X509SigningCredentials(Cert.LoadDecryptingCertificate(), relyingParty.SignatureAlgorithm, relyingParty.DigestAlgorithm),
                Subject = new ClaimsIdentity(new List<Claim> { testClaim }),
                TokenIssuerName = "https://domain.test",
                TokenType = "urn:oasis:names:tc:SAML:2.0:assertion",
                EncryptingCredentials = new EncryptedKeyEncryptingCredentials(readEncryptionCertificate)
            };

            var handler = new Saml2SecurityTokenHandler();
            var securityToken = handler.CreateToken(securityTokenDescriptor);

            var stringBuilder = new StringBuilder();
            using (var writer = XmlWriter.Create(stringBuilder))
            {
                handler.WriteToken(writer, securityToken);
            }

            var token = stringBuilder.ToString();
            
            SecurityToken validatedToken;
            var claimsPrincipal = handler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    IssuerSigningKey = new X509SecurityKey(Cert.LoadDecryptingCertificate()),
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = false,
                    ValidateAudience = false,
                    ClientDecryptionTokens =
                        new ReadOnlyCollection<SecurityToken>(new List<SecurityToken>
                        {
                            new X509SecurityToken(Cert.LoadDecryptingCertificate())
                        })
                },
                out validatedToken);

            Assert.NotNull(validatedToken);
            Assert.NotNull(claimsPrincipal);
            Assert.True(claimsPrincipal.HasClaim(testClaim.Type, testClaim.Value));
        }
    }
}