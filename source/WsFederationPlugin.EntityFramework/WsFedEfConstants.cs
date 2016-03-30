namespace WsFederationPlugin.EntityFramework
{
    class WsFedEfConstants
    {
        public const string ConnectionName = "IdentityServer3WsFederation";

        public class TableNames
        {
            public const string RelyingParty = "RelyingParties";
            public const string ClaimMap = "ClaimMappings";
        }
    }
}