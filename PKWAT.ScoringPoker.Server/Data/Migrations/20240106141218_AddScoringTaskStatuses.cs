using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PKWAT.ScoringPoker.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddScoringTaskStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ScoringTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ScoringTaskStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(19)", maxLength: 19, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoringTaskStatuses", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ScoringTaskStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "Created" },
                    { 1, "Estimation started" },
                    { 2, "Estimation finished" },
                    { 3, "Approved" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScoringTaskStatuses");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ScoringTasks");
        }
    }
}
