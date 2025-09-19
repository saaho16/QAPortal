using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QAPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedNavProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questions_QuestionsEntityId",
                table: "Answers");

            migrationBuilder.DropIndex(
                name: "IX_Answers_QuestionsEntityId",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "QuestionsEntityId",
                table: "Answers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionsEntityId",
                table: "Answers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionsEntityId",
                table: "Answers",
                column: "QuestionsEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questions_QuestionsEntityId",
                table: "Answers",
                column: "QuestionsEntityId",
                principalTable: "Questions",
                principalColumn: "Id");
        }
    }
}
