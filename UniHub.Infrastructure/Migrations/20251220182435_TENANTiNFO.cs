using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TENANTiNFO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SocialLinks",
                schema: "tenant",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Platform = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialLinks", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.CheckConstraint("CK_SocialLinks_Platform", "Platform IN (0, 1, 2, 3, 4, 5)");
                    table.ForeignKey(
                        name: "FK_SocialLinks_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "tenant",
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SupportInfos",
                schema: "tenant",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupportEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SupportPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    WorkingHours = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupportInfos_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "tenant",
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TenantInfos",
                schema: "tenant",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AlternatePhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AddressLine1 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    AddressLine2 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AboutUs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vision = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mission = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantInfos_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "tenant",
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SocialLinks_IsDeleted",
                schema: "tenant",
                table: "SocialLinks",
                column: "IsDeleted")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_SocialLinks_TenantId_Id",
                schema: "tenant",
                table: "SocialLinks",
                columns: new[] { "TenantId", "Id" })
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_SupportInfos_IsDeleted",
                schema: "tenant",
                table: "SupportInfos",
                column: "IsDeleted")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_SupportInfos_TenantId",
                schema: "tenant",
                table: "SupportInfos",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantInfos_IsDeleted",
                schema: "tenant",
                table: "TenantInfos",
                column: "IsDeleted")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_TenantInfos_TenantId",
                schema: "tenant",
                table: "TenantInfos",
                column: "TenantId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SocialLinks",
                schema: "tenant");

            migrationBuilder.DropTable(
                name: "SupportInfos",
                schema: "tenant");

            migrationBuilder.DropTable(
                name: "TenantInfos",
                schema: "tenant");
        }
    }
}
