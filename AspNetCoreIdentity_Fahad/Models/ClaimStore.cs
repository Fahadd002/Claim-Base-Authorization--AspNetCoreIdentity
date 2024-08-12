using System.Security.Claims;

namespace AspNetCoreIdentity_Fahad.Models
{
    public static class ClaimStore
    {
        public static List<Claim> GetAllClaims()
        {
            return new List<Claim>()
            { 
                new Claim("Create Role","Create Role"),
                new Claim("Edit Role","Edit Role"),
                new Claim("Delete Role","Delete Role") 
            };

        }
    }
}
