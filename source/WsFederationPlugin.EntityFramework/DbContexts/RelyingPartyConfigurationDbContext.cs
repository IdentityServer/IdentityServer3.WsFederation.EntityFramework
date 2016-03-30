using System.Collections.Specialized;
using System.Data.Entity;
using IdentityServer3.EntityFramework;
using WsFederationPlugin.EntityFramework.Entities;

namespace WsFederationPlugin.EntityFramework
{
    public class RelyingPartyConfigurationDbContext : BaseDbContext
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

        public DbSet<RelyingParty> RelyingParties { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RelyingParty>()
                .ToTable(WsFedEfConstants.TableNames.RelyingParty, Schema)
                .HasMany(x => x.ClaimMappings)
                .WithRequired(x => x.RelyingParty)
                .WillCascadeOnDelete();

            modelBuilder.Entity<ClaimMap>().ToTable(WsFedEfConstants.TableNames.ClaimMap, Schema);
        }
    }
}