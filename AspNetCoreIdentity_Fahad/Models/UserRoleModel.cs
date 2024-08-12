using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity_Fahad.Models
{
    public class UserRoleModel
    {
        [Key]
        public string UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        public bool IsSelected { get; set; }
    }
}
