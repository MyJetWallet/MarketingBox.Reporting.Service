using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketingBox.Reporting.Service.Postgres.Migrations
{
    public partial class LeadFix1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrandInfo_Brand",
                schema: "reporting-service",
                table: "leads");

            migrationBuilder.AddColumn<long>(
                name: "BrandInfo_BrandId",
                schema: "reporting-service",
                table: "leads",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrandInfo_BrandId",
                schema: "reporting-service",
                table: "leads");

            migrationBuilder.AddColumn<string>(
                name: "BrandInfo_Brand",
                schema: "reporting-service",
                table: "leads",
                type: "text",
                nullable: true);
        }
    }
}
