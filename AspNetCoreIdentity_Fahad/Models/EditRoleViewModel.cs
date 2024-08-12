using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity_Fahad.Models
{
    public class EditRoleViewModel
    {
        [Required]
        [Key]
        public string RoleId { get; set; }

        [Required]
        public string RoleName { get; set; }

        public string Description { get; set; }

        public List<string> Users { get; set; }

        public List<string> Claims { get; set; }
    }
}
