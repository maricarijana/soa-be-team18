using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Explorer.Stakeholders.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Nova : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Equipment",
                schema: "stakeholders",
                table: "People");

            migrationBuilder.DropColumn(
                name: "Level",
                schema: "stakeholders",
                table: "People");

            migrationBuilder.DropColumn(
                name: "Wallet",
                schema: "stakeholders",
                table: "People");

            migrationBuilder.DropColumn(
                name: "XP",
                schema: "stakeholders",
                table: "People");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<int>>(
                name: "Equipment",
                schema: "stakeholders",
                table: "People",
                type: "integer[]",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                schema: "stakeholders",
                table: "People",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Wallet",
                schema: "stakeholders",
                table: "People",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "XP",
                schema: "stakeholders",
                table: "People",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
