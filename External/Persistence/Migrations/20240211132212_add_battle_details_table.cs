using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class add_battle_details_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BattleDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BattleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SamuraiId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HorseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HorseRideStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HorseRideEndDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattleDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BattleDetails_Battles_BattleId",
                        column: x => x.BattleId,
                        principalTable: "Battles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BattleDetails_Horses_HorseId",
                        column: x => x.HorseId,
                        principalTable: "Horses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BattleDetails_Samurais_SamuraiId",
                        column: x => x.SamuraiId,
                        principalTable: "Samurais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BattleDetails_BattleId_SamuraiId_HorseId",
                table: "BattleDetails",
                columns: new[] { "BattleId", "SamuraiId", "HorseId" });

            migrationBuilder.CreateIndex(
                name: "IX_BattleDetails_HorseId",
                table: "BattleDetails",
                column: "HorseId");

            migrationBuilder.CreateIndex(
                name: "IX_BattleDetails_SamuraiId",
                table: "BattleDetails",
                column: "SamuraiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BattleDetails");
        }
    }
}
