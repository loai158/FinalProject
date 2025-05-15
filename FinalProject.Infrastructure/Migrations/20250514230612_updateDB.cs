using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Nurses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DoctorProfileId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NurseProfileId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PatientProfileId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nurses_IdentityUserId",
                table: "Nurses",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DoctorProfileId",
                table: "AspNetUsers",
                column: "DoctorProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_NurseProfileId",
                table: "AspNetUsers",
                column: "NurseProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PatientProfileId",
                table: "AspNetUsers",
                column: "PatientProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Doctors_DoctorProfileId",
                table: "AspNetUsers",
                column: "DoctorProfileId",
                principalTable: "Doctors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Nurses_NurseProfileId",
                table: "AspNetUsers",
                column: "NurseProfileId",
                principalTable: "Nurses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Patients_PatientProfileId",
                table: "AspNetUsers",
                column: "PatientProfileId",
                principalTable: "Patients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Nurses_AspNetUsers_IdentityUserId",
                table: "Nurses",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Doctors_DoctorProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Nurses_NurseProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Patients_PatientProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Nurses_AspNetUsers_IdentityUserId",
                table: "Nurses");

            migrationBuilder.DropIndex(
                name: "IX_Nurses_IdentityUserId",
                table: "Nurses");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DoctorProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_NurseProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PatientProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Nurses");

            migrationBuilder.DropColumn(
                name: "DoctorProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NurseProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PatientProfileId",
                table: "AspNetUsers");
        }
    }
}
