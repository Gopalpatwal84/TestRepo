namespace PowerBI.Web.Authorization
{
    public class KnownClaimTypes
    {
        public const string AppId = "appid";

        public const string TenantId = "http://schemas.microsoft.com/identity/claims/tenantid";

        public const string ObjectIdentifier = "http://schemas.microsoft.com/identity/claims/objectidentifier";

        public const string CompanyName = "http://schemas.microsoft.com/authorizationmanager/claims/companyName";

        public const string Puid = "http://schemas.microsoft.com/authorizationmanager/claims/puid";
    }
}