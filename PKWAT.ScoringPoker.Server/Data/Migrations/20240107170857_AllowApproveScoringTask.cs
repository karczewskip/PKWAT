using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PKWAT.ScoringPoker.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AllowApproveScoringTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FinalEstimationMethodName",
                table: "ScoringTasks",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FinalEstimationValue",
                table: "ScoringTasks",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalEstimationMethodName",
                table: "ScoringTasks");

            migrationBuilder.DropColumn(
                name: "FinalEstimationValue",
                table: "ScoringTasks");
        }
    }
}
