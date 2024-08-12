using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity_Fahad.Models
{
    public class RoleClaim
    {
        [Key]
        public string ClaimType { get; set; }
        public bool IsSelected { get; set; }
    }
}
