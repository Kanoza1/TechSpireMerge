using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateQuizTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Quizs_QuizId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Quizs_Level_LevelId",
                table: "Quizs");

            migrationBuilder.DropTable(
                name: "AppUserArticle");

            migrationBuilder.DropTable(
                name: "AppUserBook");

            migrationBuilder.DropTable(
                name: "AppUserQuiz");

            migrationBuilder.DropTable(
                name: "Choices");

            migrationBuilder.DropTable(
                name: "Article");

            migrationBuilder.DropTable(
                name: "Book");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Quizs",
                table: "Quizs");

            migrationBuilder.DropColumn(
                name: "QuestionAnswerIndex",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "UserAnswerIndex",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Material",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "Quizs",
                newName: "Quiz");

            migrationBuilder.RenameColumn(
                name: "AnswerTime",
                table: "Questions",
                newName: "CorrectAnswer");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Level",
                newName: "level");

            migrationBuilder.RenameColumn(
                name: "IsOtpVerified",
                table: "AspNetUsers",
                newName: "IsVerified");

            migrationBuilder.RenameIndex(
                name: "IX_Quizs_LevelId",
                table: "Quiz",
                newName: "IX_Quiz_LevelId");

            migrationBuilder.AddColumn<string>(
                name: "Answer1",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Answer2",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Answer3",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "QuizId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Quiz",
                table: "Quiz",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "QuestionsSolutions",
                columns: table => new
                {
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SolvedAt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isCorrect = table.Column<bool>(type: "bit", nullable: false),
                    QuizId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionsSolutions", x => new { x.UserId, x.QuestionId });
                    table.ForeignKey(
                        name: "FK_QuestionsSolutions_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuestionsSolutions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionsSolutions_Quiz_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quiz",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "quizezResults",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QuizId = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quizezResults", x => new { x.UserId, x.QuizId });
                    table.ForeignKey(
                        name: "FK_quizezResults_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_quizezResults_Quiz_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quiz",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_QuizId",
                table: "AspNetUsers",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsSolutions_AppUserId",
                table: "QuestionsSolutions",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsSolutions_QuestionId",
                table: "QuestionsSolutions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsSolutions_QuizId",
                table: "QuestionsSolutions",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_quizezResults_QuizId",
                table: "quizezResults",
                column: "QuizId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Quiz_QuizId",
                table: "AspNetUsers",
                column: "QuizId",
                principalTable: "Quiz",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Quiz_QuizId",
                table: "Questions",
                column: "QuizId",
                principalTable: "Quiz",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Quiz_Level_LevelId",
                table: "Quiz",
                column: "LevelId",
                principalTable: "Level",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Quiz_QuizId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Quiz_QuizId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Quiz_Level_LevelId",
                table: "Quiz");

            migrationBuilder.DropTable(
                name: "QuestionsSolutions");

            migrationBuilder.DropTable(
                name: "quizezResults");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_QuizId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Quiz",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "Answer1",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Answer2",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Answer3",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "QuizId",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "Quiz",
                newName: "Quizs");

            migrationBuilder.RenameColumn(
                name: "CorrectAnswer",
                table: "Questions",
                newName: "AnswerTime");

            migrationBuilder.RenameColumn(
                name: "level",
                table: "Level",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "IsVerified",
                table: "AspNetUsers",
                newName: "IsOtpVerified");

            migrationBuilder.RenameIndex(
                name: "IX_Quiz_LevelId",
                table: "Quizs",
                newName: "IX_Quizs_LevelId");

            migrationBuilder.AddColumn<int>(
                name: "QuestionAnswerIndex",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserAnswerIndex",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Material",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Quizs",
                table: "Quizs",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AppUserQuiz",
                columns: table => new
                {
                    QuizzesId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserQuiz", x => new { x.QuizzesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_AppUserQuiz_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserQuiz_Quizs_QuizzesId",
                        column: x => x.QuizzesId,
                        principalTable: "Quizs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Article",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Choices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    ChoiceText = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Choices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Choices_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserArticle",
                columns: table => new
                {
                    ArticlesId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserArticle", x => new { x.ArticlesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_AppUserArticle_Article_ArticlesId",
                        column: x => x.ArticlesId,
                        principalTable: "Article",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserArticle_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserBook",
                columns: table => new
                {
                    BooksId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserBook", x => new { x.BooksId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_AppUserBook_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserBook_Book_BooksId",
                        column: x => x.BooksId,
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserArticle_UsersId",
                table: "AppUserArticle",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserBook_UsersId",
                table: "AppUserBook",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserQuiz_UsersId",
                table: "AppUserQuiz",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Choices_QuestionId",
                table: "Choices",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Quizs_QuizId",
                table: "Questions",
                column: "QuizId",
                principalTable: "Quizs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quizs_Level_LevelId",
                table: "Quizs",
                column: "LevelId",
                principalTable: "Level",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
