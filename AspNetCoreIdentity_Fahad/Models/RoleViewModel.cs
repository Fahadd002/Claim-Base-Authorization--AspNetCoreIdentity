using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity_Fahad.Models
{
    public class RoleViewModel
    {
        [Required]
        [Display(Name ="Role")]
        [Key]
        public string RoleName { get; set; }

        public string Description { get; set; }
    }
}
