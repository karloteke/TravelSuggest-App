using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelSuggest.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddImageBase64ToSuggestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = 'Suggestions' AND COLUMN_NAME = 'ImageBase64'
                )
                BEGIN
                    ALTER TABLE [Suggestions] ADD [ImageBase64] nvarchar(max) NULL;
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageBase64",
                table: "Suggestions");
        }
    }
}
