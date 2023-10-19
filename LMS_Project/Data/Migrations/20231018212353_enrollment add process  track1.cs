using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class enrollmentaddprocesstrack1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AssignmentString",
                table: "ProcessTrack",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AssignmentString",
                table: "ProcessTrack",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
