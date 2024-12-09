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
                    UserId = table.Column<int>(type: "int", nullable: true),
                    ImageBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_Suggestions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Destinations",
                columns: new[] { "Id", "Category", "CityName", "Description", "ImageBase64", "IsPopular", "Season", "UserId" },
                values: new object[,]
                {
                    { 1, "Playa", "Barcelona", "Barcelona combina playas, arquitectura modernista y una vibrante vida nocturna.", null, true, "Verano", 3 },
                    { 2, "Gastronomía", "Madrid", "Madrid es conocida por sus museos de renombre, parques expansivos y una animada escena gastronómica.", null, true, "Invierno", 4 },
                    { 3, "Cultural", "Granada", "Granada alberga la impresionante Alhambra y una rica mezcla de culturas.", null, true, "Primavera", 5 },
                    { 4, "Aventura", "Londres", "Explora la aventura urbana y los emocionantes parques de Londres con actividades para todos.", null, false, "Otoño", 6 },
                    { 5, "Ocio", "Palma de Mallorca", "Conocida por sus impresionantes calas, playas y una vibrante vida nocturna.", null, true, "Todas las estaciones", 7 },
                    { 6, "Ciudad", "Ámsterdam", "Ámsterdam es famosa por sus canales, museos y una animada vida cultural.", null, true, "Primavera", 8 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Password", "Points", "Role", "UserName" },
                values: new object[,]
                {
                    { 1, "carlota@gmail.com", "carlota37", 0, "admin", "Carlota" },
                    { 2, "jesus@gmail.com", "jesus31", 0, "admin", "Jesús" },
                    { 3, "jumar@gmail.com", "jumar123", 300, "user", "Jumar" },
                    { 4, "ana@gmail.com", "ana123", 300, "user", "Ana" },
                    { 5, "nerea@gmail.com", "nerea123", 300, "user", "Nerea" },
                    { 6, "bea@gmail.com", "bea123", 300, "user", "Bea" },
                    { 7, "laura@gmail.com", "laura123", 300, "user", "Laura" },
                    { 8, "pilar@gmail.com", "pilar123", 300, "user", "Pilar" }
                });

            migrationBuilder.InsertData(
                table: "Suggestions",
                columns: new[] { "Id", "Created_at", "Description", "DestinationId", "Price", "Rating", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 12, 9, 14, 7, 24, 187, DateTimeKind.Local).AddTicks(9092), "Aprende a surfear en las famosas playas de Barcelona con instructores profesionales.", 1, 60.00m, 5, "Clases de surf en la Barceloneta", 3 },
                    { 2, new DateTime(2024, 12, 9, 14, 7, 24, 187, DateTimeKind.Local).AddTicks(9102), "Degusta una variedad de tapas y especialidades locales en el histórico Mercado de San Miguel.", 2, 50.00m, 5, "Tour gastronómico por el Mercado de San Miguel", 4 },
                    { 3, new DateTime(2024, 12, 9, 14, 7, 24, 187, DateTimeKind.Local).AddTicks(9110), "Explora los majestuosos palacios y jardines de la Alhambra con un guía experto.", 3, 30.00m, 5, "Visita guiada a la Alhambra", 5 },
                    { 4, new DateTime(2024, 12, 9, 14, 7, 24, 187, DateTimeKind.Local).AddTicks(9117), "Disfruta de impresionantes vistas de Londres mientras recorres las rutas de Hampstead Heath.", 4, 25.00m, 4, "Senderismo en Hampstead Heath", 6 },
                    { 5, new DateTime(2024, 12, 9, 14, 7, 24, 187, DateTimeKind.Local).AddTicks(9124), "Descubre calas escondidas y disfruta de aguas cristalinas en un paseo en barco.", 5, 150.00m, 5, "Paseo en barco por las calas de Mallorca", 7 },
                    { 6, new DateTime(2024, 12, 9, 14, 7, 24, 187, DateTimeKind.Local).AddTicks(9132), "Explora Ámsterdam como un local en un tour en bicicleta por sus canales históricos.", 6, 40.00m, 4, "Tour en bicicleta por los canales", 8 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Suggestions_DestinationId",
                table: "Suggestions",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Suggestions_UserId",
                table: "Suggestions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Suggestions");

            migrationBuilder.DropTable(
                name: "Destinations");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
