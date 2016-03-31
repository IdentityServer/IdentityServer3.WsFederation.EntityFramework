using System;
using System.Data.Entity;
using System.Threading.Tasks;
using IdentityServer3.WsFederation.Models;
using IdentityServer3.WsFederation.Services;
using WsFederationPlugin.EntityFramework.Extensions;
using WsFederationPlugin.EntityFramework.Interfaces;

namespace WsFederationPlugin.EntityFramework.Services
{
    public class RelyingPartyService :IRelyingPartyService
    {
        private readonly IRelyingPartyConfigurationDbContext context;

        public RelyingPartyService(IRelyingPartyConfigurationDbContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            this.context = context;
        }

        public async Task<RelyingParty> GetByRealmAsync(string realm)
        {
            var relyingParty = await context.RelyingParties
                .Include(x => x.ClaimMappings)
                .SingleOrDefaultAsync(x => x.Realm == realm);

            var model = relyingParty.ToModel();

            return model;
        }
    }
}