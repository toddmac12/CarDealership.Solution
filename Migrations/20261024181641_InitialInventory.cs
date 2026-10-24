using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarDealership.Migrations
{
    /// <inheritdoc />
    public partial class InitialInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CarModel = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    Price = table.Column<int>(type: "INTEGER", nullable: false),
                    Miles = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "CarModel", "Miles", "Price" },
                values: new object[,]
                {
                    { 1, "2019 Subaru Forester", 68000, 17500 },
                    { 2, "2021 Toyota Camry", 42000, 22900 },
                    { 3, "2020 Honda Civic", 51000, 18900 },
                    { 4, "2018 Ford F-150", 87000, 24900 },
                    { 5, "2022 Hyundai Tucson", 29000, 25900 },
                    { 6, "2020 Chevrolet Equinox", 62000, 17900 },
                    { 7, "2021 Mazda CX-5", 38000, 23900 },
                    { 8, "2017 Jeep Wrangler", 91000, 21900 },
                    { 9, "2022 Kia Soul", 33000, 16900 },
                    { 10, "2019 Nissan Altima", 72000, 15900 },
                    { 11, "2023 Tesla Model 3", 18000, 28900 },
                    { 12, "2018 Volkswagen Golf", 83000, 13900 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cars");
        }
    }
}
