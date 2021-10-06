using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MarketingBox.Reporting.Service.Postgres.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "reporting-service");

            migrationBuilder.CreateTable(
                name: "leads",
                schema: "reporting-service",
                columns: table => new
                {
                    LeadId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<string>(type: "text", nullable: true),
                    UniqueId = table.Column<string>(type: "text", nullable: true),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    Ip = table.Column<string>(type: "text", nullable: true),
                    BrandInfo_AffiliateId = table.Column<long>(type: "bigint", nullable: true),
                    BrandInfo_CampaignId = table.Column<long>(type: "bigint", nullable: true),
                    BrandInfo_BoxId = table.Column<long>(type: "bigint", nullable: true),
                    BrandInfo_Brand = table.Column<string>(type: "text", nullable: true),
                    AdditionalInfo_So = table.Column<string>(type: "text", nullable: true),
                    AdditionalInfo_Sub = table.Column<string>(type: "text", nullable: true),
                    AdditionalInfo_Sub1 = table.Column<string>(type: "text", nullable: true),
                    AdditionalInfo_Sub2 = table.Column<string>(type: "text", nullable: true),
                    AdditionalInfo_Sub3 = table.Column<string>(type: "text", nullable: true),
                    AdditionalInfo_Sub4 = table.Column<string>(type: "text", nullable: true),
                    AdditionalInfo_Sub5 = table.Column<string>(type: "text", nullable: true),
                    AdditionalInfo_Sub6 = table.Column<string>(type: "text", nullable: true),
                    AdditionalInfo_Sub7 = table.Column<string>(type: "text", nullable: true),
                    AdditionalInfo_Sub8 = table.Column<string>(type: "text", nullable: true),
                    AdditionalInfo_Sub9 = table.Column<string>(type: "text", nullable: true),
                    AdditionalInfo_Sub10 = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Sequence = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_leads", x => x.LeadId);
                });

            migrationBuilder.CreateTable(
                name: "reports",
                schema: "reporting-service",
                columns: table => new
                {
                    AffiliateId = table.Column<long>(type: "bigint", nullable: false),
                    LeadId = table.Column<long>(type: "bigint", nullable: false),
                    ReportType = table.Column<int>(type: "integer", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: true),
                    UniqueId = table.Column<string>(type: "text", nullable: true),
                    BoxId = table.Column<long>(type: "bigint", nullable: false),
                    CampaignId = table.Column<long>(type: "bigint", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Payout = table.Column<decimal>(type: "numeric", nullable: false),
                    Revenue = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reports", x => new { x.AffiliateId, x.LeadId, x.ReportType });
                });

            migrationBuilder.CreateIndex(
                name: "IX_leads_TenantId_LeadId",
                schema: "reporting-service",
                table: "leads",
                columns: new[] { "TenantId", "LeadId" });

            migrationBuilder.CreateIndex(
                name: "IX_reports_CreatedAt",
                schema: "reporting-service",
                table: "reports",
                column: "CreatedAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "leads",
                schema: "reporting-service");

            migrationBuilder.DropTable(
                name: "reports",
                schema: "reporting-service");
        }
    }
}
