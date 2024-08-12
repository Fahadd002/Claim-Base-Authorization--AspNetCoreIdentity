using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity_Fahad.Models
{
    public class RoleClaimViewModel
    {
        public RoleClaimViewModel()
        {
            claims = new List<RoleClaim>();
        }
        [Key]
        public string RoleId { get; set; }

        public List<RoleClaim> claims { get;set; }
    }
}
