using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addmedicnetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dose",
                table: "Perscribtions");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Perscribtions");

            migrationBuilder.CreateTable(
                name: "Medicines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dose = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicinePerscribtion",
                columns: table => new
                {
                    MedicinesId = table.Column<int>(type: "int", nullable: false),
                    PerscribtionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicinePerscribtion", x => new { x.MedicinesId, x.PerscribtionsId });
                    table.ForeignKey(
                        name: "FK_MedicinePerscribtion_Medicines_MedicinesId",
                        column: x => x.MedicinesId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicinePerscribtion_Perscribtions_PerscribtionsId",
                        column: x => x.PerscribtionsId,
                        principalTable: "Perscribtions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicinePerscribtion_PerscribtionsId",
                table: "MedicinePerscribtion",
                column: "PerscribtionsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicinePerscribtion");

            migrationBuilder.DropTable(
                name: "Medicines");

            migrationBuilder.AddColumn<string>(
                name: "Dose",
                table: "Perscribtions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Perscribtions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
