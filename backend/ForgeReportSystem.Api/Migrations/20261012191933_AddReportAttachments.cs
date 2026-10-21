using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForgeReportSystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddReportAttachments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AttachmentFileName",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttachmentFilePath",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentFileName",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "AttachmentFilePath",
                table: "Reports");
        }
    }
}
