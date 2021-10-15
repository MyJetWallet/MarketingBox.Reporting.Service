using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MarketingBox.Reporting.Service.Postgres.Migrations
{
    public partial class DepositKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_deposits",
                schema: "reporting-service",
                table: "deposits");

            migrationBuilder.AddColumn<long>(
                name: "DepositId",
                schema: "reporting-service",
                table: "deposits",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_deposits",
                schema: "reporting-service",
                table: "deposits",
                column: "DepositId");

            migrationBuilder.CreateIndex(
                name: "IX_deposits_LeadId",
                schema: "reporting-service",
                table: "deposits",
                column: "LeadId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_deposits",
                schema: "reporting-service",
                table: "deposits");

            migrationBuilder.DropIndex(
                name: "IX_deposits_LeadId",
                schema: "reporting-service",
                table: "deposits");

            migrationBuilder.DropColumn(
                name: "DepositId",
                schema: "reporting-service",
                table: "deposits");

            migrationBuilder.AddPrimaryKey(
                name: "PK_deposits",
                schema: "reporting-service",
                table: "deposits",
                column: "LeadId");
        }
    }
}
