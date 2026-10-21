using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeHunt.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedUniqueLocationIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Locations_City_Neighborhood",
                table: "Locations",
                columns: new[] { "City", "Neighborhood" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Locations_City_Neighborhood",
                table: "Locations");
        }
    }
}
