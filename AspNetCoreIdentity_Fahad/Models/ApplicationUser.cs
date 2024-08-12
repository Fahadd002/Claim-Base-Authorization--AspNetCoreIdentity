using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentity_Fahad.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
    }
}
