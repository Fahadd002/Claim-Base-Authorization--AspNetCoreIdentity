using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AspNetCoreIdentity_Fahad.Models;

namespace AspNetCoreIdentity_Fahad.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<AspNetCoreIdentity_Fahad.Models.RegisterViewModel> RegisterViewModel { get; set; } = default!;
        public DbSet<AspNetCoreIdentity_Fahad.Models.LoginViewModel> LoginViewModel { get; set; } = default!;
        public DbSet<AspNetCoreIdentity_Fahad.Models.RoleViewModel> RoleViewModel { get; set; } = default!;
        public DbSet<AspNetCoreIdentity_Fahad.Models.EditRoleViewModel> EditRoleViewModel { get; set; } = default!;
        public DbSet<AspNetCoreIdentity_Fahad.Models.EditUserViewModel> EditUserViewModel { get; set; } = default!;
        public DbSet<AspNetCoreIdentity_Fahad.Models.UserClaimViewModel> UserClaimViewModel { get; set; } = default!;
        public DbSet<AspNetCoreIdentity_Fahad.Models.RoleClaimViewModel> RoleClaimViewModel { get; set; } = default!;
    }
}
