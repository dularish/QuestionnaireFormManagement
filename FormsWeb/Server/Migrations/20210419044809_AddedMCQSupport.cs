using Microsoft.EntityFrameworkCore.Migrations;

namespace FormsWeb.Server.Migrations
{
    public partial class AddedMCQSupport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ResponseText",
                table: "Responses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ResponseTypeAllowed",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MultipleChoiceSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultipleChoiceSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MultipleChoiceSets_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultipleChoice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MultipleChoiceSetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultipleChoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MultipleChoice_MultipleChoiceSets_MultipleChoiceSetId",
                        column: x => x.MultipleChoiceSetId,
                        principalTable: "MultipleChoiceSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResponseMultipleChoiceSelectionMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResponseId = table.Column<int>(type: "int", nullable: false),
                    MultipleChoiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponseMultipleChoiceSelectionMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResponseMultipleChoiceSelectionMap_MultipleChoice_MultipleChoiceId",
                        column: x => x.MultipleChoiceId,
                        principalTable: "MultipleChoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResponseMultipleChoiceSelectionMap_Responses_ResponseId",
                        column: x => x.ResponseId,
                        principalTable: "Responses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MultipleChoice_MultipleChoiceSetId",
                table: "MultipleChoice",
                column: "MultipleChoiceSetId");

            migrationBuilder.CreateIndex(
                name: "IX_MultipleChoiceSets_QuestionId",
                table: "MultipleChoiceSets",
                column: "QuestionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResponseMultipleChoiceSelectionMap_MultipleChoiceId",
                table: "ResponseMultipleChoiceSelectionMap",
                column: "MultipleChoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponseMultipleChoiceSelectionMap_ResponseId",
                table: "ResponseMultipleChoiceSelectionMap",
                column: "ResponseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResponseMultipleChoiceSelectionMap");

            migrationBuilder.DropTable(
                name: "MultipleChoice");

            migrationBuilder.DropTable(
                name: "MultipleChoiceSets");

            migrationBuilder.DropColumn(
                name: "ResponseTypeAllowed",
                table: "Questions");

            migrationBuilder.AlterColumn<string>(
                name: "ResponseText",
                table: "Responses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
