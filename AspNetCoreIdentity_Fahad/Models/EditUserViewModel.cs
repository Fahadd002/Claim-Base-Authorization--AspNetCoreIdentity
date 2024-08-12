using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity_Fahad.Models
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Claims = new List<string>();
            Roles=new List<string>();
        }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        [Key] 
        public string Email { get; set; }

        public string Id { get; set; }

        public string UserName { get; set; }

        public List<string> Claims { get; set; }

        public List<string> Roles { get; set; }
    }
}
