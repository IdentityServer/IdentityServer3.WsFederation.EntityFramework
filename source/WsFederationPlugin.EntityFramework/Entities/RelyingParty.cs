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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WsFederationPlugin.EntityFramework.Entities
{
    public class RelyingParty
    {
        [Key]
        public virtual int Id { get; set; }

        [Required]
        [StringLength(2000)]
        [Index(IsUnique = true)]
        public virtual string Realm { get; set; }

        [StringLength(200)]
        public virtual string Name { get; set; }

        public virtual bool Enabled { get; set; }

        [Required]
        [StringLength(2000)]
        public virtual string ReplyUrl { get; set; }

        public virtual string TokenType { get; set; }

        [Range(0, int.MaxValue)]
        public virtual int TokenLifeTime { get; set; }

        public virtual byte[] EncryptingCertificate { get; set; }

        public virtual bool IncludeAllClaimsForUser { get; set; }

        public virtual string DefaultClaimTypeMappingPrefix { get; set; }

        public virtual string SamlNameIdentifierFormat { get; set; }

        public virtual ICollection<ClaimMap> ClaimMappings { get; set; }

        public virtual string SignatureAlgorithm { get; set; }

        public virtual string DigestAlgorithm { get; set; }
    }
}