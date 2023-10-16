using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyZen.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CreatedBy_User = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy_TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy_User = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy_TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lectures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                    CreatedBy_User = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy_TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy_User = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy_TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lectures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lectures_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlashcardSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LectureId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false),
                    CreatedBy_User = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy_TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy_User = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy_TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashcardSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlashcardSets_Lectures_LectureId",
                        column: x => x.LectureId,
                        principalTable: "Lectures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Flashcards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlashcardSetId = table.Column<int>(type: "int", nullable: false),
                    Front = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Back = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CreatedBy_User = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy_TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy_User = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy_TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flashcards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flashcards_FlashcardSets_FlashcardSetId",
                        column: x => x.FlashcardSetId,
                        principalTable: "FlashcardSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flashcards_FlashcardSetId",
                table: "Flashcards",
                column: "FlashcardSetId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashcardSets_LectureId",
                table: "FlashcardSets",
                column: "LectureId");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_CourseId",
                table: "Lectures",
                column: "CourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flashcards");

            migrationBuilder.DropTable(
                name: "FlashcardSets");

            migrationBuilder.DropTable(
                name: "Lectures");

            migrationBuilder.DropTable(
                name: "Courses");
        }
    }
}
