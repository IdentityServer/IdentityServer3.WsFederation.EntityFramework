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