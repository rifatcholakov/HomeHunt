using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeHunt.Data.Migrations
{
    /// <inheritdoc />
    public partial class NormalizedSeededAgentPhones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: 1,
                column: "Phone",
                value: "+359888100001");

            migrationBuilder.UpdateData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: 2,
                column: "Phone",
                value: "+359888100002");

            migrationBuilder.UpdateData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: 3,
                column: "Phone",
                value: "+359888100003");

            migrationBuilder.UpdateData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: 4,
                column: "Phone",
                value: "+359888100004");

            migrationBuilder.UpdateData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: 5,
                column: "Phone",
                value: "+359888100005");

            migrationBuilder.UpdateData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: 6,
                column: "Phone",
                value: "+359888100006");

            migrationBuilder.UpdateData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: 7,
                column: "Phone",
                value: "+359888100007");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: 1,
                column: "Phone",
                value: "+359 888 100 001");

            migrationBuilder.UpdateData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: 2,
                column: "Phone",
                value: "+359 888 100 002");

            migrationBuilder.UpdateData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: 3,
                column: "Phone",
                value: "+359 888 100 003");

            migrationBuilder.UpdateData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: 4,
                column: "Phone",
                value: "+359 888 100 004");

            migrationBuilder.UpdateData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: 5,
                column: "Phone",
                value: "+359 888 100 005");

            migrationBuilder.UpdateData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: 6,
                column: "Phone",
                value: "+359 888 100 006");

            migrationBuilder.UpdateData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: 7,
                column: "Phone",
                value: "+359 888 100 007");
        }
    }
}
