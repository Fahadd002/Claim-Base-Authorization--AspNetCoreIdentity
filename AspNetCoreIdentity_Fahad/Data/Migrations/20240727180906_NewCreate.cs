using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetCoreIdentity_Fahad.Data.Migrations
{
    /// <inheritdoc />
    public partial class NewCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Claims",
                table: "EditRoleViewModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "Users",
                table: "EditRoleViewModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.CreateTable(
                name: "EditUserViewModel",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Claims = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Roles = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EditUserViewModel", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaimViewModel",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaimViewModel", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "UserClaimViewModel",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaimViewModel", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaim",
                columns: table => new
                {
                    ClaimType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsSelected = table.Column<bool>(type: "bit", nullable: false),
                    RoleClaimViewModelRoleId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaim", x => x.ClaimType);
                    table.ForeignKey(
                        name: "FK_RoleClaim_RoleClaimViewModel_RoleClaimViewModelRoleId",
                        column: x => x.RoleClaimViewModelRoleId,
                        principalTable: "RoleClaimViewModel",
                        principalColumn: "RoleId");
                });

            migrationBuilder.CreateTable(
                name: "UserClaim",
                columns: table => new
                {
                    ClaimType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsSeleted = table.Column<bool>(type: "bit", nullable: false),
                    UserClaimViewModelUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaim", x => x.ClaimType);
                    table.ForeignKey(
                        name: "FK_UserClaim_UserClaimViewModel_UserClaimViewModelUserId",
                        column: x => x.UserClaimViewModelUserId,
                        principalTable: "UserClaimViewModel",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaim_RoleClaimViewModelRoleId",
                table: "RoleClaim",
                column: "RoleClaimViewModelRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaim_UserClaimViewModelUserId",
                table: "UserClaim",
                column: "UserClaimViewModelUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EditUserViewModel");

            migrationBuilder.DropTable(
                name: "RoleClaim");

            migrationBuilder.DropTable(
                name: "UserClaim");

            migrationBuilder.DropTable(
                name: "RoleClaimViewModel");

            migrationBuilder.DropTable(
                name: "UserClaimViewModel");

            migrationBuilder.DropColumn(
                name: "Claims",
                table: "EditRoleViewModel");

            migrationBuilder.DropColumn(
                name: "Users",
                table: "EditRoleViewModel");
        }
    }
}
