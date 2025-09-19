using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QAPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedNavProps2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Users_CreatedBy",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Approvals_Users_UserId",
                table: "Approvals");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Users_CreatedBy",
                table: "Questions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Questions",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Users_CreatedBy",
                table: "Questions",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Users_CreatedBy",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Approvals_Users_UserId",
                table: "Approvals");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Users_CreatedBy",
                table: "Questions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Questions",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "GETDATE()");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Users_CreatedBy",
                table: "Questions",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
