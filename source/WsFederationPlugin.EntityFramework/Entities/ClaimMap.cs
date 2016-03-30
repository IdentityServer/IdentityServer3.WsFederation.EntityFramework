using System.ComponentModel.DataAnnotations;

namespace WsFederationPlugin.EntityFramework.Entities
{
    public class ClaimMap
    {
        [Key]
        public virtual int Id { get; set; }

        [Required]
        [StringLength(250)]
        public virtual string InboundClaim { get; set; }

        [Required]
        [StringLength(250)]
        public virtual string OutboundClaim { get; set; }

        public virtual RelyingParty RelyingParty { get; set; }
    }
}