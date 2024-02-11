using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class add_index_unique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BattleDetails_BattleId_SamuraiId_HorseId",
                table: "BattleDetails");

            migrationBuilder.AlterColumn<DateTime>(
                name: "HorseRideEndDate",
                table: "BattleDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BattleDetails_BattleId_SamuraiId_HorseId",
                table: "BattleDetails",
                columns: new[] { "BattleId", "SamuraiId", "HorseId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BattleDetails_BattleId_SamuraiId_HorseId",
                table: "BattleDetails");

            migrationBuilder.AlterColumn<DateTime>(
                name: "HorseRideEndDate",
                table: "BattleDetails",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_BattleDetails_BattleId_SamuraiId_HorseId",
                table: "BattleDetails",
                columns: new[] { "BattleId", "SamuraiId", "HorseId" });
        }
    }
}
