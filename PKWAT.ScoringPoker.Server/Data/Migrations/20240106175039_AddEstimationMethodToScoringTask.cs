using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PKWAT.ScoringPoker.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEstimationMethodToScoringTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EstimationMethodId",
                table: "ScoringTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ScoringTasks_EstimationMethodId",
                table: "ScoringTasks",
                column: "EstimationMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScoringTasks_EstimationMethods_EstimationMethodId",
                table: "ScoringTasks",
                column: "EstimationMethodId",
                principalTable: "EstimationMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScoringTasks_EstimationMethods_EstimationMethodId",
                table: "ScoringTasks");

            migrationBuilder.DropIndex(
                name: "IX_ScoringTasks_EstimationMethodId",
                table: "ScoringTasks");

            migrationBuilder.DropColumn(
                name: "EstimationMethodId",
                table: "ScoringTasks");
        }
    }
}
