using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "email");

            migrationBuilder.CreateTable(
                name: "EmailLogs",
                schema: "email",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_EmailLogs", x => x.Id);
                    table.CheckConstraint("CK_EmailLogs_Status", "[Status] IN (0, 1)");
                    table.ForeignKey(
                        name: "FK_EmailLogs_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "tenant",
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmailTemplates",
                schema: "email",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailTemplates_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "tenant",
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmailReciever",
                schema: "email",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmailLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_EmailReciever", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailReciever_EmailLogs_EmailLogId",
                        column: x => x.EmailLogId,
                        principalSchema: "email",
                        principalTable: "EmailLogs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmailReciever_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalSchema: "tenant",
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailLogs_IsDeleted",
                schema: "email",
                table: "EmailLogs",
                column: "IsDeleted")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_EmailLogs_TenantId",
                schema: "email",
                table: "EmailLogs",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailReciever_EmailLogId",
                schema: "email",
                table: "EmailReciever",
                column: "EmailLogId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailReciever_IsDeleted",
                schema: "email",
                table: "EmailReciever",
                column: "IsDeleted")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_EmailReciever_TenantId",
                schema: "email",
                table: "EmailReciever",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplates_IsDeleted",
                schema: "email",
                table: "EmailTemplates",
                column: "IsDeleted")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplates_TenantId",
                schema: "email",
                table: "EmailTemplates",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailReciever",
                schema: "email");

            migrationBuilder.DropTable(
                name: "EmailTemplates",
                schema: "email");

            migrationBuilder.DropTable(
                name: "EmailLogs",
                schema: "email");
        }
    }
}
