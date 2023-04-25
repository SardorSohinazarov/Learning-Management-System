using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.API.Context.Migrations
{
    /// <inheritdoc />
    public partial class LocalizationStringsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocalizedStrings",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Uz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ru = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    En = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalizedStrings", x => x.Key);
                });

            migrationBuilder.InsertData(
                table: "LocalizedStrings",
                columns: new[] { "Key", "En", "Ru", "Uz" },
                values: new object[] { "Required", "{0} failed is required", "{0} ... ruscha ...", "{0} kiritilishi shart" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocalizedStrings");
        }
    }
}
