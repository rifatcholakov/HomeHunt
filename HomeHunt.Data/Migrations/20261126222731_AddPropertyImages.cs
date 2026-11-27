using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeHunt.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertyImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PropertyImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    PublicId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PropertyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyImages_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PropertyImages_PropertyId",
                table: "PropertyImages",
                column: "PropertyId");

            migrationBuilder.Sql(
                @"INSERT INTO PropertyImages (Url, PropertyId)
                  SELECT ImageUrl, Id
                  FROM Properties
                  WHERE ImageUrl IS NOT NULL AND ImageUrl <> ''");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Properties");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Properties",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.Sql(
                @"UPDATE p
                  SET ImageUrl = (SELECT TOP 1 i.Url FROM PropertyImages i WHERE i.PropertyId = p.Id ORDER BY i.Id)
                  FROM Properties p");

            migrationBuilder.DropTable(
                name: "PropertyImages");
        }
    }
}
