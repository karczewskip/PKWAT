using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PKWAT.ScoringPoker.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddScoringTaskTimes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EstimationStarted",
                table: "ScoringTasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduledEstimationFinish",
                table: "ScoringTasks",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimationStarted",
                table: "ScoringTasks");

            migrationBuilder.DropColumn(
                name: "ScheduledEstimationFinish",
                table: "ScoringTasks");
        }
    }
}
