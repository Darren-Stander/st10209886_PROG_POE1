using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace st10209886_PROG_POE1.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClaimFileModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupportingDocument",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "ClaimFiles");

            migrationBuilder.AddColumn<byte[]>(
                name: "FileData",
                table: "ClaimFiles",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileData",
                table: "ClaimFiles");

            migrationBuilder.AddColumn<string>(
                name: "SupportingDocument",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "ClaimFiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
