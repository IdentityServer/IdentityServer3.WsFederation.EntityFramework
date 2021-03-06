﻿/*
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

using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer3.EntityFramework;
using IdentityServer3.WsFederation.Services;
using IdentityServer3.WsFederation.EntityFramework.Entities;
using RelyingParty = IdentityServer3.WsFederation.Models.RelyingParty;

namespace IdentityServer3.WsFederation.EntityFramework
{
    public class RelyingPartyService : IRelyingPartyService
    {
        private readonly EntityFrameworkServiceOptions options;
        private readonly IRelyingPartyConfigurationDbContext context;

        public RelyingPartyService(IRelyingPartyConfigurationDbContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            this.context = context;
        }

        public RelyingPartyService(EntityFrameworkServiceOptions options, IRelyingPartyConfigurationDbContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            this.options = options;
            this.context = context;
        }

        public async Task<RelyingParty> GetByRealmAsync(string realm)
        {
            Entities.RelyingParty relyingParty;
            var queriable = context.RelyingParties
                .Include(x => x.ClaimMappings)
                .Include(x => x.PostLogoutRedirectUris);

            if (options != null && options.SynchronousReads)
            {
                relyingParty = queriable.SingleOrDefault(x => x.Realm == realm);
            }
            else
            {
                relyingParty = await queriable.SingleOrDefaultAsync(x => x.Realm == realm);
            }

            var model = relyingParty.ToModel();

            return model;
        }
    }
}