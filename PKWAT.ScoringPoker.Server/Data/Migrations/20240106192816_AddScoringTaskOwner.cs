using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PKWAT.ScoringPoker.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddScoringTaskOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "ScoringTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "ScoringTasks");
        }
    }
}
