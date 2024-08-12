using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentity_Fahad.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }
    }
}
