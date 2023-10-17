using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyZen.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLectureSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlashcardSets_Lectures_LectureId",
                table: "FlashcardSets");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy_TimeStamp",
                table: "Lectures",
                newName: "UpdatedBy_Timestamp");

            migrationBuilder.RenameColumn(
                name: "CreatedBy_TimeStamp",
                table: "Lectures",
                newName: "CreatedBy_Timestamp");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy_TimeStamp",
                table: "FlashcardSets",
                newName: "UpdatedBy_Timestamp");

            migrationBuilder.RenameColumn(
                name: "CreatedBy_TimeStamp",
                table: "FlashcardSets",
                newName: "CreatedBy_Timestamp");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy_TimeStamp",
                table: "Flashcards",
                newName: "UpdatedBy_Timestamp");

            migrationBuilder.RenameColumn(
                name: "CreatedBy_TimeStamp",
                table: "Flashcards",
                newName: "CreatedBy_Timestamp");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy_TimeStamp",
                table: "Courses",
                newName: "UpdatedBy_Timestamp");

            migrationBuilder.RenameColumn(
                name: "CreatedBy_TimeStamp",
                table: "Courses",
                newName: "CreatedBy_Timestamp");

            migrationBuilder.AlterColumn<int>(
                name: "LectureId",
                table: "FlashcardSets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_FlashcardSets_Lectures_LectureId",
                table: "FlashcardSets",
                column: "LectureId",
                principalTable: "Lectures",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlashcardSets_Lectures_LectureId",
                table: "FlashcardSets");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy_Timestamp",
                table: "Lectures",
                newName: "UpdatedBy_TimeStamp");

            migrationBuilder.RenameColumn(
                name: "CreatedBy_Timestamp",
                table: "Lectures",
                newName: "CreatedBy_TimeStamp");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy_Timestamp",
                table: "FlashcardSets",
                newName: "UpdatedBy_TimeStamp");

            migrationBuilder.RenameColumn(
                name: "CreatedBy_Timestamp",
                table: "FlashcardSets",
                newName: "CreatedBy_TimeStamp");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy_Timestamp",
                table: "Flashcards",
                newName: "UpdatedBy_TimeStamp");

            migrationBuilder.RenameColumn(
                name: "CreatedBy_Timestamp",
                table: "Flashcards",
                newName: "CreatedBy_TimeStamp");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy_Timestamp",
                table: "Courses",
                newName: "UpdatedBy_TimeStamp");

            migrationBuilder.RenameColumn(
                name: "CreatedBy_Timestamp",
                table: "Courses",
                newName: "CreatedBy_TimeStamp");

            migrationBuilder.AlterColumn<int>(
                name: "LectureId",
                table: "FlashcardSets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FlashcardSets_Lectures_LectureId",
                table: "FlashcardSets",
                column: "LectureId",
                principalTable: "Lectures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
