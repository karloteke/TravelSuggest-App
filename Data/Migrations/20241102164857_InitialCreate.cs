using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TravelSuggest.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Destinations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Season = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPopular = table.Column<bool>(type: "bit", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suggestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    DestinationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suggestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Suggestions_Destinations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "Destinations",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Destinations",
                columns: new[] { "Id", "Category", "CityName", "Description", "IsPopular", "Season", "UserId" },
                values: new object[,]
                {
                    { 1, "Ocio", "Londres", "Explora la rica historia, cultura y entretenimiento que ofrece Londres, desde museos gratuitos hasta vibrantes mercados y parques.", false, "Todo el año", 2 },
                    { 2, "Gastronómica", "París", "La capital francesa es famosa por su exquisita gastronomía, desde panaderías hasta restaurantes de alta cocina.", true, "Primavera", 3 },
                    { 3, "Cultural", "Ámsterdam", "Ámsterdam es conocida por sus museos, arquitectura y vibrante escena artística.", true, "Verano", 4 },
                    { 4, "Aventura", "Mallorca", "La isla de Mallorca ofrece una amplia variedad de actividades al aire libre, desde senderismo hasta deportes acuáticos.", true, "Verano", 5 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Password", "Points", "Role", "UserName" },
                values: new object[,]
                {
                    { 1, "carlota@gmail.com", "Carlota36", 0, "admin", "Carlota" },
                    { 2, "ana@gmail.com", "Ana123", 300, "user", "Ana" },
                    { 3, "jesus@gmail.com", "Jesus30", 300, "user", "Jesus" },
                    { 4, "paola@gmail.com", "Paola123", 300, "user", "Paola" },
                    { 5, "pilar@gmail.com", "Pilar123", 300, "user", "Pilar" }
                });

            migrationBuilder.InsertData(
                table: "Suggestions",
                columns: new[] { "Id", "Created_at", "Description", "DestinationId", "Price", "Rating", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 11, 2, 17, 48, 56, 639, DateTimeKind.Local).AddTicks(1717), "Disfruta de una experiencia única viendo los principales monumentos de Londres desde el agua.", 1, 25.00m, 4, "Tour en barco por el río Támesis", 3 },
                    { 2, new DateTime(2024, 11, 2, 17, 48, 56, 639, DateTimeKind.Local).AddTicks(1723), "Explora las delicias culinarias de París mientras paseas por uno de los barrios más emblemáticos de la ciudad.", 2, 60.00m, 4, "Tour gastronómico por Montmartre", 5 },
                    { 3, new DateTime(2024, 11, 2, 17, 48, 56, 639, DateTimeKind.Local).AddTicks(1728), "Explora la ciudad sobre dos ruedas y descubre su belleza única.", 3, 15.00m, 4, "Recorrido en bicicleta por los canales", 2 },
                    { 4, new DateTime(2024, 11, 2, 17, 48, 56, 639, DateTimeKind.Local).AddTicks(1733), "Descubre calas escondidas y cuevas marinas en un tour de motos de agua guiado.", 4, 150.00m, 5, "Tour motos de agua en Cala Gamba", 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Suggestions_DestinationId",
                table: "Suggestions",
                column: "DestinationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Suggestions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Destinations");
        }
    }
}
