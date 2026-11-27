using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeHunt.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddImageSortOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "PropertyImages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(
                @"UPDATE i
                  SET SortOrder = ranked.Position
                  FROM PropertyImages i
                  INNER JOIN (
                      SELECT Id, ROW_NUMBER() OVER (PARTITION BY PropertyId ORDER BY Id) - 1 AS Position
                      FROM PropertyImages
                  ) ranked ON ranked.Id = i.Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "PropertyImages");
        }
    }
}
