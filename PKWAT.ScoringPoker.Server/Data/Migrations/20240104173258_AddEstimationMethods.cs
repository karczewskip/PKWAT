using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PKWAT.ScoringPoker.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEstimationMethods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstimationMethods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstimationMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstimationMethodPossibleValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstimationMethodId = table.Column<int>(type: "int", nullable: false),
                    EstimationMethodValue = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstimationMethodPossibleValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstimationMethodPossibleValues_EstimationMethods_EstimationMethodId",
                        column: x => x.EstimationMethodId,
                        principalTable: "EstimationMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EstimationMethodPossibleValues_EstimationMethodId",
                table: "EstimationMethodPossibleValues",
                column: "EstimationMethodId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstimationMethodPossibleValues");

            migrationBuilder.DropTable(
                name: "EstimationMethods");
        }
    }
}
