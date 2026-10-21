using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HomeHunt.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Agents",
                columns: new[] { "Id", "AgencyName", "Email", "FirstName", "LastName", "Phone" },
                values: new object[,]
                {
                    { 1, "Sunrise Estates", "ivan.petrov@example.com", "Ivan", "Petrov", "+359 888 100 001" },
                    { 2, "Sunrise Estates", "maria.georgieva@example.com", "Maria", "Georgieva", "+359 888 100 002" },
                    { 3, "Balkan Homes", "georgi.dimitrov@example.com", "Georgi", "Dimitrov", "+359 888 100 003" },
                    { 4, "Balkan Homes", "elena.stoyanova@example.com", "Elena", "Stoyanova", "+359 888 100 004" },
                    { 5, "Black Sea Realty", "nikolay.ivanov@example.com", "Nikolay", "Ivanov", "+359 888 100 005" },
                    { 6, "Black Sea Realty", "desislava.koleva@example.com", "Desislava", "Koleva", "+359 888 100 006" },
                    { 7, "Rhodope Properties", "stefan.hristov@example.com", "Stefan", "Hristov", "+359 888 100 007" }
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "City", "Neighborhood", "PostalCode" },
                values: new object[,]
                {
                    { 1, "Sofia", "Lozenets", "1164" },
                    { 2, "Sofia", "Mladost", "1750" },
                    { 3, "Sofia", "Lyulin", "1360" },
                    { 4, "Plovdiv", "Kapana", "4000" },
                    { 5, "Plovdiv", "Trakia", "4023" },
                    { 6, "Varna", "Chaika", "9010" },
                    { 7, "Varna", "Briz", "9027" },
                    { 8, "Burgas", "Lazur", "8000" },
                    { 9, "Blagoevgrad", "Varosha", "2700" }
                });

            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "AgentId", "Area", "Bathrooms", "Bedrooms", "CreatedOn", "Description", "ImageUrl", "ListingType", "LocationId", "Price", "PropertyType", "Status", "Title" },
                values: new object[,]
                {
                    { 1, 1, 82m, 1, 2, new DateTime(2026, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Renovated apartment close to the park with a large living room, a balcony and a parking spot in the garage.", "https://images.unsplash.com/photo-1522708323590-d24dbb6b0267?auto=format&fit=crop&w=800&q=70", 0, 1, 185000m, 0, 0, "Bright two-bedroom apartment in Lozenets" },
                    { 2, 1, 36m, 1, 1, new DateTime(2026, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fully furnished studio with a small kitchen and a new bathroom. A five minute walk to the metro station.", "https://images.unsplash.com/photo-1493809842364-78817add7ffb?auto=format&fit=crop&w=800&q=70", 1, 2, 650m, 2, 0, "Modern studio near the metro" },
                    { 3, 2, 105m, 2, 3, new DateTime(2026, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Large apartment on the fifth floor with a lift, two terraces and a view of Vitosha mountain.", null, 0, 2, 158000m, 0, 1, "Three-bedroom apartment in Mladost" },
                    { 4, 3, 160m, 2, 0, new DateTime(2026, 9, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Open space office with two meeting rooms, air conditioning and four parking places.", "https://images.unsplash.com/photo-1497366216548-37526070297c?auto=format&fit=crop&w=800&q=70", 1, 2, 2400m, 3, 0, "Office space in Business Park" },
                    { 5, 4, 70m, 1, 2, new DateTime(2026, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unfurnished apartment with central heating, ready to move in. Pets are allowed.", "https://images.unsplash.com/photo-1554995207-c18c203602cb?auto=format&fit=crop&w=800&q=70", 1, 3, 520m, 0, 3, "Two-bedroom apartment for rent in Lyulin" },
                    { 6, 4, 78m, 1, 1, new DateTime(2026, 9, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Creative loft in the old creative quarter of Plovdiv with high ceilings, exposed bricks and a lot of natural light.", "https://images.unsplash.com/photo-1536376072261-38c75010e6c9?auto=format&fit=crop&w=800&q=70", 0, 4, 139000m, 0, 0, "Loft in the heart of Kapana" },
                    { 7, 3, 45m, 1, 0, new DateTime(2026, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ground floor shop with a big window on a busy pedestrian street. Suitable for a cafe or a boutique.", "https://images.unsplash.com/photo-1441986300917-64674bd600d8?auto=format&fit=crop&w=800&q=70", 1, 4, 1100m, 4, 0, "Small shop on the main street" },
                    { 8, 5, 95m, 2, 2, new DateTime(2026, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Two-bedroom apartment with a big terrace and a view of the sea. The building has an elevator and a guarded parking.", "https://images.unsplash.com/photo-1560448204-e02f11c3d0e2?auto=format&fit=crop&w=800&q=70", 0, 6, 245000m, 0, 0, "Sea view apartment in Chaika" },
                    { 9, 5, 310m, 4, 5, new DateTime(2026, 9, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Modern house with a swimming pool, three bathrooms and a smart heating system. Quiet area near the forest.", "https://images.unsplash.com/photo-1613977257363-707ba9348227?auto=format&fit=crop&w=800&q=70", 0, 7, 560000m, 1, 1, "Luxury house in Briz" },
                    { 10, 6, 76m, 1, 2, new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Two-bedroom apartment next to the Sea Garden, on the third floor of a well maintained building.", "https://images.unsplash.com/photo-1460317442991-0ec209397118?auto=format&fit=crop&w=800&q=70", 0, 8, 128000m, 0, 2, "Apartment in Lazur near the park" },
                    { 11, 5, 600m, 0, 0, new DateTime(2026, 9, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Flat plot of land in a developing area with electricity and water at the border. Permission for a house up to three floors.", null, 0, 7, 95000m, 5, 0, "Building plot in Briz" },
                    { 12, 7, 150m, 2, 3, new DateTime(2026, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Old house in good condition, with a big yard and a view of Pirin mountain. Needs a small renovation.", "https://images.unsplash.com/photo-1464146072230-91cabc968266?auto=format&fit=crop&w=800&q=70", 0, 9, 175000m, 1, 0, "House with mountain view in Varosha" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 9);
        }
    }
}
