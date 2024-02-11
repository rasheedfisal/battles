using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class add_unique_key_fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BattleDetails_BattleId_SamuraiId_HorseId",
                table: "BattleDetails");

            migrationBuilder.DropIndex(
                name: "IX_BattleDetails_SamuraiId",
                table: "BattleDetails");

            migrationBuilder.CreateIndex(
                name: "IX_BattleDetails_BattleId",
                table: "BattleDetails",
                column: "BattleId");

            migrationBuilder.CreateIndex(
                name: "IX_BattleDetails_SamuraiId_HorseId",
                table: "BattleDetails",
                columns: new[] { "SamuraiId", "HorseId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BattleDetails_BattleId",
                table: "BattleDetails");

            migrationBuilder.DropIndex(
                name: "IX_BattleDetails_SamuraiId_HorseId",
                table: "BattleDetails");

            migrationBuilder.CreateIndex(
                name: "IX_BattleDetails_BattleId_SamuraiId_HorseId",
                table: "BattleDetails",
                columns: new[] { "BattleId", "SamuraiId", "HorseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BattleDetails_SamuraiId",
                table: "BattleDetails",
                column: "SamuraiId");
        }
    }
}
