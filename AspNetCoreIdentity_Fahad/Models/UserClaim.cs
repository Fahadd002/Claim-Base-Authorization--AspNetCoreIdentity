using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity_Fahad.Models
{
    public class UserClaim
    {
        [Key]
        public string ClaimType { get; set; }
        public bool IsSeleted { get; set; }
    }
}
