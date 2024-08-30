using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServicesDomicile.Migrations
{
    /// <inheritdoc />
    public partial class AddCoverImageToGig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverImage",
                table: "Gigs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverImage",
                table: "Gigs");
        }
    }
}
