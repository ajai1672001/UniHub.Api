using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterTenantUserAndAspNetUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Gender",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TimeZone",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                schema: "tenant",
                table: "TenantUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "tenant",
                table: "TenantUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                schema: "tenant",
                table: "TenantUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "tenant",
                table: "TenantUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                schema: "tenant",
                table: "TenantUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "TimeZone",
                schema: "tenant",
                table: "TenantUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TenantUsers_RoleId",
                schema: "tenant",
                table: "TenantUsers",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_TenantUsers_AspNetRoles_RoleId",
                schema: "tenant",
                table: "TenantUsers",
                column: "RoleId",
                principalSchema: "identity",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TenantUsers_AspNetRoles_RoleId",
                schema: "tenant",
                table: "TenantUsers");

            migrationBuilder.DropIndex(
                name: "IX_TenantUsers_RoleId",
                schema: "tenant",
                table: "TenantUsers");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                schema: "tenant",
                table: "TenantUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "tenant",
                table: "TenantUsers");

            migrationBuilder.DropColumn(
                name: "Gender",
                schema: "tenant",
                table: "TenantUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "tenant",
                table: "TenantUsers");

            migrationBuilder.DropColumn(
                name: "RoleId",
                schema: "tenant",
                table: "TenantUsers");

            migrationBuilder.DropColumn(
                name: "TimeZone",
                schema: "tenant",
                table: "TenantUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                schema: "identity",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "identity",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                schema: "identity",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "identity",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TimeZone",
                schema: "identity",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}