using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace st10209886_PROG_POE1.Migrations
{
    /// <inheritdoc />
    public partial class AddAdditionalNotesField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalNotes",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalNotes",
                table: "Claims");
        }
    }
}
