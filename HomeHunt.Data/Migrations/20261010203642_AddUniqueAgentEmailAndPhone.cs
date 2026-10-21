using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeHunt.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueAgentEmailAndPhone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Agents_Email",
                table: "Agents",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agents_Phone",
                table: "Agents",
                column: "Phone",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Agents_Email",
                table: "Agents");

            migrationBuilder.DropIndex(
                name: "IX_Agents_Phone",
                table: "Agents");
        }
    }
}
