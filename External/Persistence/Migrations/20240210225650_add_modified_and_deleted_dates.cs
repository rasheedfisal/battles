using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class add_modified_and_deleted_dates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                table: "Samurais",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOnUtc",
                table: "Samurais",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                table: "Horses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOnUtc",
                table: "Horses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                table: "Battles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOnUtc",
                table: "Battles",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                table: "Samurais");

            migrationBuilder.DropColumn(
                name: "ModifiedOnUtc",
                table: "Samurais");

            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                table: "Horses");

            migrationBuilder.DropColumn(
                name: "ModifiedOnUtc",
                table: "Horses");

            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                table: "Battles");

            migrationBuilder.DropColumn(
                name: "ModifiedOnUtc",
                table: "Battles");
        }
    }
}
