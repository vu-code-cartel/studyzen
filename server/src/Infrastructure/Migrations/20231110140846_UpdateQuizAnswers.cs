using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyZen.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateQuizAnswers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizQuestions_QuizAnswers_CorrectAnswerId",
                table: "QuizQuestions");

            migrationBuilder.DropIndex(
                name: "IX_QuizQuestions_CorrectAnswerId",
                table: "QuizQuestions");

            migrationBuilder.DropColumn(
                name: "CorrectAnswerId",
                table: "QuizQuestions");

            migrationBuilder.AddColumn<bool>(
                name: "IsCorrect",
                table: "QuizAnswers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCorrect",
                table: "QuizAnswers");

            migrationBuilder.AddColumn<int>(
                name: "CorrectAnswerId",
                table: "QuizQuestions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestions_CorrectAnswerId",
                table: "QuizQuestions",
                column: "CorrectAnswerId",
                unique: true,
                filter: "[CorrectAnswerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizQuestions_QuizAnswers_CorrectAnswerId",
                table: "QuizQuestions",
                column: "CorrectAnswerId",
                principalTable: "QuizAnswers",
                principalColumn: "Id");
        }
    }
}
