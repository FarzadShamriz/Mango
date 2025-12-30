using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.Services.EmailAPI.Migrations
{
    /// <inheritdoc />
    public partial class Capitalizedthetablename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_emailLoggers",
                table: "emailLoggers");

            migrationBuilder.RenameTable(
                name: "emailLoggers",
                newName: "EmailLoggers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmailLoggers",
                table: "EmailLoggers",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmailLoggers",
                table: "EmailLoggers");

            migrationBuilder.RenameTable(
                name: "EmailLoggers",
                newName: "emailLoggers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_emailLoggers",
                table: "emailLoggers",
                column: "Id");
        }
    }
}
