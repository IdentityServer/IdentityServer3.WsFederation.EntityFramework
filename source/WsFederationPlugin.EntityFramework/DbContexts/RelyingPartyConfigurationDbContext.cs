/*
 * Copyright 2014 Dominick Baier, Brock Allen
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections.Specialized;
using System.Data.Entity;
using IdentityServer3.EntityFramework;
using IdentityServer3.WsFederation.EntityFramework.Entities;

namespace IdentityServer3.WsFederation.EntityFramework
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

            modelBuilder.Entity<RelyingParty>()
                .HasMany(x => x.PostLogoutRedirectUris)
                .WithRequired(x => x.RelyingParty)
                .WillCascadeOnDelete();

            modelBuilder.Entity<RelyingParty>().Property(x => x.EncryptingCertificate).IsOptional();

            modelBuilder.Entity<ClaimMap>().ToTable(WsFedEfConstants.TableNames.ClaimMap, Schema);
            modelBuilder.Entity<RelyingPartyPostLogoutUri>()
                .ToTable(WsFedEfConstants.TableNames.RelyinPartyPostLogoutRedirectUris, Schema);
        }
    }
}