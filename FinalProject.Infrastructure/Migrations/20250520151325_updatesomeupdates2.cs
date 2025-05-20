using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatesomeupdates2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PerscribtionMedicines_Medicines_MedicineId",
                table: "PerscribtionMedicines");

            migrationBuilder.DropIndex(
                name: "IX_Perscribtions_AppointmentId",
                table: "Perscribtions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PerscribtionMedicines",
                table: "PerscribtionMedicines");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "PerscribtionMedicines",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PerscribtionMedicines",
                table: "PerscribtionMedicines",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Perscribtions_AppointmentId",
                table: "Perscribtions",
                column: "AppointmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PerscribtionMedicines_PerscribtionId",
                table: "PerscribtionMedicines",
                column: "PerscribtionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PerscribtionMedicines_Medicines_MedicineId",
                table: "PerscribtionMedicines",
                column: "MedicineId",
                principalTable: "Medicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PerscribtionMedicines_Medicines_MedicineId",
                table: "PerscribtionMedicines");

            migrationBuilder.DropIndex(
                name: "IX_Perscribtions_AppointmentId",
                table: "Perscribtions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PerscribtionMedicines",
                table: "PerscribtionMedicines");

            migrationBuilder.DropIndex(
                name: "IX_PerscribtionMedicines_PerscribtionId",
                table: "PerscribtionMedicines");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PerscribtionMedicines");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PerscribtionMedicines",
                table: "PerscribtionMedicines",
                columns: new[] { "PerscribtionId", "MedicineId" });

            migrationBuilder.CreateIndex(
                name: "IX_Perscribtions_AppointmentId",
                table: "Perscribtions",
                column: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_PerscribtionMedicines_Medicines_MedicineId",
                table: "PerscribtionMedicines",
                column: "MedicineId",
                principalTable: "Medicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
