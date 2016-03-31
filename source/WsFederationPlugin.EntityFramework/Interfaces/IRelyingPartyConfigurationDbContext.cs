using System.Data.Entity;
using WsFederationPlugin.EntityFramework.Entities;

namespace WsFederationPlugin.EntityFramework.Interfaces
{
    public interface IRelyingPartyConfigurationDbContext
    {
        DbSet<RelyingParty> RelyingParties { get; set; }
    }
}