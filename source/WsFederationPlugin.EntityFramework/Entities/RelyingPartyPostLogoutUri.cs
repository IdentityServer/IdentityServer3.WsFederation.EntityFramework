using System.ComponentModel.DataAnnotations;

namespace IdentityServer3.WsFederation.EntityFramework.Entities
{
    public class RelyingPartyPostLogoutUri
    {
        [Key]
        public virtual int Id { get; set; }

        [Required]
        [StringLength(2000)]
        public virtual string Uri { get; set; }

        public virtual RelyingParty RelyingParty { get; set; }
    }
}