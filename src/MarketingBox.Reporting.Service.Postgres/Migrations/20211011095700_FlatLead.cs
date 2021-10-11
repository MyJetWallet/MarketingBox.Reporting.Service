using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketingBox.Reporting.Service.Postgres.Migrations
{
    public partial class FlatLead : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BrandInfo_CampaignId",
                schema: "reporting-service",
                table: "leads",
                newName: "CampaignId");

            migrationBuilder.RenameColumn(
                name: "BrandInfo_BrandId",
                schema: "reporting-service",
                table: "leads",
                newName: "BrandId");

            migrationBuilder.RenameColumn(
                name: "BrandInfo_BoxId",
                schema: "reporting-service",
                table: "leads",
                newName: "BoxId");

            migrationBuilder.RenameColumn(
                name: "BrandInfo_AffiliateId",
                schema: "reporting-service",
                table: "leads",
                newName: "AffiliateId");

            migrationBuilder.RenameColumn(
                name: "AdditionalInfo_Sub9",
                schema: "reporting-service",
                table: "leads",
                newName: "Sub9");

            migrationBuilder.RenameColumn(
                name: "AdditionalInfo_Sub8",
                schema: "reporting-service",
                table: "leads",
                newName: "Sub8");

            migrationBuilder.RenameColumn(
                name: "AdditionalInfo_Sub7",
                schema: "reporting-service",
                table: "leads",
                newName: "Sub7");

            migrationBuilder.RenameColumn(
                name: "AdditionalInfo_Sub6",
                schema: "reporting-service",
                table: "leads",
                newName: "Sub6");

            migrationBuilder.RenameColumn(
                name: "AdditionalInfo_Sub5",
                schema: "reporting-service",
                table: "leads",
                newName: "Sub5");

            migrationBuilder.RenameColumn(
                name: "AdditionalInfo_Sub4",
                schema: "reporting-service",
                table: "leads",
                newName: "Sub4");

            migrationBuilder.RenameColumn(
                name: "AdditionalInfo_Sub3",
                schema: "reporting-service",
                table: "leads",
                newName: "Sub3");

            migrationBuilder.RenameColumn(
                name: "AdditionalInfo_Sub2",
                schema: "reporting-service",
                table: "leads",
                newName: "Sub2");

            migrationBuilder.RenameColumn(
                name: "AdditionalInfo_Sub10",
                schema: "reporting-service",
                table: "leads",
                newName: "Sub10");

            migrationBuilder.RenameColumn(
                name: "AdditionalInfo_Sub1",
                schema: "reporting-service",
                table: "leads",
                newName: "Sub1");

            migrationBuilder.RenameColumn(
                name: "AdditionalInfo_Sub",
                schema: "reporting-service",
                table: "leads",
                newName: "Sub");

            migrationBuilder.RenameColumn(
                name: "AdditionalInfo_So",
                schema: "reporting-service",
                table: "leads",
                newName: "So");

            migrationBuilder.AlterColumn<long>(
                name: "CampaignId",
                schema: "reporting-service",
                table: "leads",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "BrandId",
                schema: "reporting-service",
                table: "leads",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "BoxId",
                schema: "reporting-service",
                table: "leads",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "AffiliateId",
                schema: "reporting-service",
                table: "leads",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_leads_AffiliateId",
                schema: "reporting-service",
                table: "leads",
                column: "AffiliateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_leads_AffiliateId",
                schema: "reporting-service",
                table: "leads");

            migrationBuilder.RenameColumn(
                name: "Sub9",
                schema: "reporting-service",
                table: "leads",
                newName: "AdditionalInfo_Sub9");

            migrationBuilder.RenameColumn(
                name: "Sub8",
                schema: "reporting-service",
                table: "leads",
                newName: "AdditionalInfo_Sub8");

            migrationBuilder.RenameColumn(
                name: "Sub7",
                schema: "reporting-service",
                table: "leads",
                newName: "AdditionalInfo_Sub7");

            migrationBuilder.RenameColumn(
                name: "Sub6",
                schema: "reporting-service",
                table: "leads",
                newName: "AdditionalInfo_Sub6");

            migrationBuilder.RenameColumn(
                name: "Sub5",
                schema: "reporting-service",
                table: "leads",
                newName: "AdditionalInfo_Sub5");

            migrationBuilder.RenameColumn(
                name: "Sub4",
                schema: "reporting-service",
                table: "leads",
                newName: "AdditionalInfo_Sub4");

            migrationBuilder.RenameColumn(
                name: "Sub3",
                schema: "reporting-service",
                table: "leads",
                newName: "AdditionalInfo_Sub3");

            migrationBuilder.RenameColumn(
                name: "Sub2",
                schema: "reporting-service",
                table: "leads",
                newName: "AdditionalInfo_Sub2");

            migrationBuilder.RenameColumn(
                name: "Sub10",
                schema: "reporting-service",
                table: "leads",
                newName: "AdditionalInfo_Sub10");

            migrationBuilder.RenameColumn(
                name: "Sub1",
                schema: "reporting-service",
                table: "leads",
                newName: "AdditionalInfo_Sub1");

            migrationBuilder.RenameColumn(
                name: "Sub",
                schema: "reporting-service",
                table: "leads",
                newName: "AdditionalInfo_Sub");

            migrationBuilder.RenameColumn(
                name: "So",
                schema: "reporting-service",
                table: "leads",
                newName: "AdditionalInfo_So");

            migrationBuilder.RenameColumn(
                name: "CampaignId",
                schema: "reporting-service",
                table: "leads",
                newName: "BrandInfo_CampaignId");

            migrationBuilder.RenameColumn(
                name: "BrandId",
                schema: "reporting-service",
                table: "leads",
                newName: "BrandInfo_BrandId");

            migrationBuilder.RenameColumn(
                name: "BoxId",
                schema: "reporting-service",
                table: "leads",
                newName: "BrandInfo_BoxId");

            migrationBuilder.RenameColumn(
                name: "AffiliateId",
                schema: "reporting-service",
                table: "leads",
                newName: "BrandInfo_AffiliateId");

            migrationBuilder.AlterColumn<long>(
                name: "BrandInfo_CampaignId",
                schema: "reporting-service",
                table: "leads",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "BrandInfo_BrandId",
                schema: "reporting-service",
                table: "leads",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "BrandInfo_BoxId",
                schema: "reporting-service",
                table: "leads",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "BrandInfo_AffiliateId",
                schema: "reporting-service",
                table: "leads",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
