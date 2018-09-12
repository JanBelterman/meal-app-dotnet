using Microsoft.EntityFrameworkCore.Migrations;

namespace MaaltijdApplicatie.Migrations
{
    public partial class Addrelationshipsbetweenmealandstudents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StudentCookId",
                table: "Meals",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MealStudents",
                columns: table => new
                {
                    AppUserId = table.Column<string>(nullable: false),
                    MealId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealStudents", x => new { x.MealId, x.AppUserId });
                    table.ForeignKey(
                        name: "FK_MealStudents_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealStudents_Meals_MealId",
                        column: x => x.MealId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Meals_DateTime",
                table: "Meals",
                column: "DateTime",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Meals_StudentCookId",
                table: "Meals",
                column: "StudentCookId");

            migrationBuilder.CreateIndex(
                name: "IX_MealStudents_AppUserId",
                table: "MealStudents",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meals_AspNetUsers_StudentCookId",
                table: "Meals",
                column: "StudentCookId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meals_AspNetUsers_StudentCookId",
                table: "Meals");

            migrationBuilder.DropTable(
                name: "MealStudents");

            migrationBuilder.DropIndex(
                name: "IX_Meals_DateTime",
                table: "Meals");

            migrationBuilder.DropIndex(
                name: "IX_Meals_StudentCookId",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "StudentCookId",
                table: "Meals");
        }
    }
}
