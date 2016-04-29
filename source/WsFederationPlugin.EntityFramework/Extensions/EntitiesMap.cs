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
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using IdentityServer3.WsFederation.Models;

namespace WsFederationPlugin.EntityFramework.Extensions
{
    public static class EntitiesMap
    {
        static EntitiesMap()
        {
            Mapper =
                new MapperConfiguration(
                    config =>
                    {
                        config.CreateMap<Entities.RelyingParty, RelyingParty>(MemberList.Destination)
                            .ForMember(x => x.DefaultClaimTypeMappingPrefix,
                                opt => opt.MapFrom(src => src.DefaultClaimTypeMappingPrefix))
                            .ForMember(x => x.DigestAlgorithm,
                                opt => opt.MapFrom(src => src.DigestAlgorithm))
                            .ForMember(x => x.Enabled, opt => opt.MapFrom(src => src.Enabled))
                            .ForMember(x => x.IncludeAllClaimsForUser,
                                opt => opt.MapFrom(src => src.IncludeAllClaimsForUser))
                            .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
                            .ForMember(x => x.Realm, opt => opt.MapFrom(src => src.Realm))
                            .ForMember(x => x.ReplyUrl, opt => opt.MapFrom(src => src.ReplyUrl))
                            .ForMember(x => x.SamlNameIdentifierFormat,
                                opt => opt.MapFrom(src => src.SamlNameIdentifierFormat))
                            .ForMember(x => x.SignatureAlgorithm, opt => opt.MapFrom(src => src.SignatureAlgorithm))
                            .ForMember(x => x.TokenLifeTime, opt => opt.MapFrom(src => src.TokenLifeTime))
                            .ForMember(x => x.TokenType, opt => opt.MapFrom(src => src.TokenType))
                            .ForMember(x => x.EncryptingCertificate,
                                opt =>
                                    opt.MapFrom(
                                        src =>
                                            src.EncryptingCertificate != null
                                                ? new X509Certificate2(src.EncryptingCertificate)
                                                : null))
                            .ForMember(x => x.ClaimMappings,
                                opt =>
                                    opt.MapFrom(
                                        src =>
                                            src.ClaimMappings.GroupBy(x => x.InboundClaim)
                                                .ToDictionary(x => x.Key, x => x.First().OutboundClaim,
                                                    StringComparer.OrdinalIgnoreCase)
                                        )).ForAllMembers(x => x.Condition(src => !src.IsSourceValueNull));
                    })
                    .CreateMapper();
        }

        public static IMapper Mapper { get; set; }

        public static RelyingParty ToModel(this Entities.RelyingParty relyingParty)
        {
            if (relyingParty == null) return null;
            return Mapper.Map<Entities.RelyingParty, RelyingParty>(relyingParty);
        }
    }
}