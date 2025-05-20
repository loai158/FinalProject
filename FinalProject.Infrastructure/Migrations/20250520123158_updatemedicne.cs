using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatemedicne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicinePerscribtion");

            migrationBuilder.CreateTable(
                name: "PerscribtionMedicines",
                columns: table => new
                {
                    PerscribtionId = table.Column<int>(type: "int", nullable: false),
                    MedicineId = table.Column<int>(type: "int", nullable: false),
                    Dose = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerscribtionMedicines", x => new { x.PerscribtionId, x.MedicineId });
                    table.ForeignKey(
                        name: "FK_PerscribtionMedicines_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PerscribtionMedicines_Perscribtions_PerscribtionId",
                        column: x => x.PerscribtionId,
                        principalTable: "Perscribtions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PerscribtionMedicines_MedicineId",
                table: "PerscribtionMedicines",
                column: "MedicineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PerscribtionMedicines");

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
    }
}
