using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class alterEmailTemplateTenant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailLogs_Tenants_TenantId",
                schema: "email",
                table: "EmailLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailReciever_Tenants_TenantId",
                schema: "email",
                table: "EmailReciever");

            migrationBuilder.DropIndex(
                name: "IX_EmailReciever_TenantId",
                schema: "email",
                table: "EmailReciever");

            migrationBuilder.DropIndex(
                name: "IX_EmailLogs_TenantId",
                schema: "email",
                table: "EmailLogs");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "email",
                table: "EmailReciever");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "email",
                table: "EmailLogs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                schema: "email",
                table: "EmailReciever",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                schema: "email",
                table: "EmailLogs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_EmailReciever_TenantId",
                schema: "email",
                table: "EmailReciever",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailLogs_TenantId",
                schema: "email",
                table: "EmailLogs",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailLogs_Tenants_TenantId",
                schema: "email",
                table: "EmailLogs",
                column: "TenantId",
                principalSchema: "tenant",
                principalTable: "Tenants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailReciever_Tenants_TenantId",
                schema: "email",
                table: "EmailReciever",
                column: "TenantId",
                principalSchema: "tenant",
                principalTable: "Tenants",
                principalColumn: "Id");
        }
    }
}
