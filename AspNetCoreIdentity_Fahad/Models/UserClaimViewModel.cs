using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity_Fahad.Models
{
    public class UserClaimViewModel
    {
        public UserClaimViewModel()
        {
            Claims = new List<UserClaim>();
        }

        [Key]
        public string UserId { get; set; }
        public List<UserClaim> Claims { get; set; }

    }
}
