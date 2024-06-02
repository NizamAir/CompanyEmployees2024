using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CompanyEmployees2024.Migrations
{
    /// <inheritdoc />
    public partial class ReviewAddShiftId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<Guid>(
                name: "ShiftId",
                table: "Reviews",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ShiftId",
                table: "Reviews",
                column: "ShiftId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Shifts_ShiftId",
                table: "Reviews",
                column: "ShiftId",
                principalTable: "Shifts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Shifts_ShiftId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ShiftId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ShiftId",
                table: "Reviews");

            
        }
    }
}
