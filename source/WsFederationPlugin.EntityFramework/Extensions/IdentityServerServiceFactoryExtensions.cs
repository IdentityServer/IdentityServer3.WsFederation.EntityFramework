using System;
using IdentityServer3.Core.Configuration;
using IdentityServer3.EntityFramework;
using IdentityServer3.WsFederation.Configuration;
using IdentityServer3.WsFederation.Services;
using WsFederationPlugin.EntityFramework.Interfaces;
using WsFederationPlugin.EntityFramework.Services;

namespace WsFederationPlugin.EntityFramework.Extensions
{
    public static class WsFederationServiceFactoryExtensions
    {
        public static void RegisterRelyingPartyService(this WsFederationServiceFactory factory,
            EntityFrameworkServiceOptions options)
        {
            if (factory == null) throw new ArgumentNullException("factory");
            if (options == null) throw new ArgumentNullException("options");

            factory.Register(new Registration<IRelyingPartyConfigurationDbContext>(resolver => new RelyingPartyConfigurationDbContext(options.ConnectionString, options.Schema)));
            factory.RelyingPartyService = new Registration<IRelyingPartyService, RelyingPartyService>();
        }
    }
}