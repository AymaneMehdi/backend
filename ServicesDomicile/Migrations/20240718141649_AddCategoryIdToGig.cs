using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServicesDomicile.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryIdToGig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "Gigs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Gigs_CategoryId",
                table: "Gigs",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gigs_Categories_CategoryId",
                table: "Gigs",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gigs_Categories_CategoryId",
                table: "Gigs");

            migrationBuilder.DropIndex(
                name: "IX_Gigs_CategoryId",
                table: "Gigs");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Gigs");
        }
    }
}
