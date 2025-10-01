using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class tenantUserPrimary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrimary",
                schema: "tenant",
                table: "TenantUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrimary",
                schema: "tenant",
                table: "TenantUsers");
        }
    }
}
