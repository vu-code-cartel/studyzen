using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyZen.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixFlashcardSetLectureFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlashcardSets_Lectures_LectureId",
                table: "FlashcardSets");

            migrationBuilder.AddForeignKey(
                name: "FK_FlashcardSets_Lectures_LectureId",
                table: "FlashcardSets",
                column: "LectureId",
                principalTable: "Lectures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlashcardSets_Lectures_LectureId",
                table: "FlashcardSets");

            migrationBuilder.AddForeignKey(
                name: "FK_FlashcardSets_Lectures_LectureId",
                table: "FlashcardSets",
                column: "LectureId",
                principalTable: "Lectures",
                principalColumn: "Id");
        }
    }
}
