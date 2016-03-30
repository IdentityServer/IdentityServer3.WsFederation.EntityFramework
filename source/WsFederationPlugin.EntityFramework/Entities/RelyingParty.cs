using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WsFederationPlugin.EntityFramework.Entities
{
    public class RelyingParty
    {
        [Key]
        public virtual int Id { get; set; }

        [Required]
        [StringLength(2000)]
        [Index(IsUnique = true)]
        public virtual string Realm { get; set; }

        [StringLength(200)]
        public virtual string Name { get; set; }

        public virtual bool Enabled { get; set; }

        [Required]
        [StringLength(2000)]
        public virtual string ReplyUrl { get; set; }

        public virtual string TokenType { get; set; }

        [Range(0, int.MaxValue)]
        public virtual int TokenLifeTime { get; set; }

        public virtual byte[] EncryptingCertificate { get; set; }

        public virtual bool IncludeAllClaimsForUser { get; set; }

        public virtual string DefaultClaimTypeMappingPrefix { get; set; }

        public virtual string SamlNameIdentifierFormat { get; set; }

        public virtual ICollection<ClaimMap> ClaimMappings { get; set; }

        public virtual string SignatureAlgorithm { get; set; }

        public virtual string DigestAlgorithm { get; set; }
    }
}