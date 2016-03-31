using System.Collections.Specialized;
using System.Data.Entity;
using IdentityServer3.EntityFramework;
using WsFederationPlugin.EntityFramework.Entities;
using WsFederationPlugin.EntityFramework.Interfaces;

namespace WsFederationPlugin.EntityFramework
{
    public class RelyingPartyConfigurationDbContext : BaseDbContext, IRelyingPartyConfigurationDbContext
    {
        public RelyingPartyConfigurationDbContext() : base(WsFedEfConstants.ConnectionName)
        {
        }

        public RelyingPartyConfigurationDbContext(string connectionString) : base(connectionString)
        {
        }

        public RelyingPartyConfigurationDbContext(string connectionString, string schema)
            : base(connectionString, schema)
        {
        }

        public DbSet<RelyingParty> RelyingParties { get; set; }

        protected override void ConfigureChildCollections()
        {
            Set<RelyingParty>().Local.CollectionChanged +=
                delegate(object sender, NotifyCollectionChangedEventArgs e)
                {
                    if (e.Action == NotifyCollectionChangedAction.Add)
                    {
                        foreach (RelyingParty item in e.NewItems)
                        {
                            RegisterDeleteOnRemove(item.ClaimMappings);
                        }
                    }
                };
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RelyingParty>()
                .ToTable(WsFedEfConstants.TableNames.RelyingParty, Schema)
                .HasMany(x => x.ClaimMappings)
                .WithRequired(x => x.RelyingParty)
                .WillCascadeOnDelete();

            modelBuilder.Entity<RelyingParty>().Property(x => x.EncryptingCertificate).IsOptional();

            modelBuilder.Entity<ClaimMap>().ToTable(WsFedEfConstants.TableNames.ClaimMap, Schema);
        }
    }
}