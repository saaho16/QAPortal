using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QAPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedNavPropsAndRel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questions_QuestionId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Users_CreatedBy",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Approvals_Users_UserId",
                table: "Approvals");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questions_QuestionId",
                table: "Answers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Users_CreatedBy",
                table: "Answers",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Approvals_Users_UserId",
                table: "Approvals",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questions_QuestionId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Users_CreatedBy",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Approvals_Users_UserId",
                table: "Approvals");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questions_QuestionId",
                table: "Answers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Users_CreatedBy",
                table: "Answers",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Approvals_Users_UserId",
                table: "Approvals",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
