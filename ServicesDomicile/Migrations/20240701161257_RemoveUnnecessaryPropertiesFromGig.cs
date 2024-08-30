using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServicesDomicile.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnnecessaryPropertiesFromGig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                table: "Gigs");

            migrationBuilder.DropColumn(
                name: "Features",
                table: "Gigs");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Gigs");

            migrationBuilder.DropColumn(
                name: "RevisionNumber",
                table: "Gigs");

            migrationBuilder.DropColumn(
                name: "StarNumber",
                table: "Gigs");

            migrationBuilder.DropColumn(
                name: "TotalStars",
                table: "Gigs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeliveryDate",
                table: "Gigs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Features",
                table: "Gigs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Gigs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "RevisionNumber",
                table: "Gigs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StarNumber",
                table: "Gigs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalStars",
                table: "Gigs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
