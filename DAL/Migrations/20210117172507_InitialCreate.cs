using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameBoard",
                columns: table => new
                {
                    GameBoardId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Height = table.Column<int>(type: "INTEGER", nullable: false),
                    Width = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameBoard", x => x.GameBoardId);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OwnBoardGameBoardId = table.Column<int>(type: "INTEGER", nullable: true),
                    AttackBoardGameBoardId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_Players_GameBoard_AttackBoardGameBoardId",
                        column: x => x.AttackBoardGameBoardId,
                        principalTable: "GameBoard",
                        principalColumn: "GameBoardId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Players_GameBoard_OwnBoardGameBoardId",
                        column: x => x.OwnBoardGameBoardId,
                        principalTable: "GameBoard",
                        principalColumn: "GameBoardId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cell",
                columns: table => new
                {
                    CellId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    XPosition = table.Column<int>(type: "INTEGER", nullable: false),
                    YPosition = table.Column<int>(type: "INTEGER", nullable: false),
                    _state = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: true),
                    PlayerId1 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cell", x => x.CellId);
                    table.ForeignKey(
                        name: "FK_Cell_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cell_Players_PlayerId1",
                        column: x => x.PlayerId1,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GameStates",
                columns: table => new
                {
                    GameStateId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CurrentMoveByPlayerOne = table.Column<bool>(type: "INTEGER", nullable: false),
                    PlayerOnePlayerId = table.Column<int>(type: "INTEGER", nullable: true),
                    PlayerTwoPlayerId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameStates", x => x.GameStateId);
                    table.ForeignKey(
                        name: "FK_GameStates_Players_PlayerOnePlayerId",
                        column: x => x.PlayerOnePlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameStates_Players_PlayerTwoPlayerId",
                        column: x => x.PlayerTwoPlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cell_PlayerId",
                table: "Cell",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Cell_PlayerId1",
                table: "Cell",
                column: "PlayerId1");

            migrationBuilder.CreateIndex(
                name: "IX_GameStates_PlayerOnePlayerId",
                table: "GameStates",
                column: "PlayerOnePlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_GameStates_PlayerTwoPlayerId",
                table: "GameStates",
                column: "PlayerTwoPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_AttackBoardGameBoardId",
                table: "Players",
                column: "AttackBoardGameBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_OwnBoardGameBoardId",
                table: "Players",
                column: "OwnBoardGameBoardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cell");

            migrationBuilder.DropTable(
                name: "GameStates");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "GameBoard");
        }
    }
}
