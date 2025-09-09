using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlataformaEstagios.Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    ApplicationIdentifier = table.Column<Guid>(type: "uuid", nullable: false),
                    VacancyId = table.Column<Guid>(type: "uuid", nullable: false),
                    CandidateIdentifier = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CandidateId = table.Column<long>(type: "bigint", nullable: true),
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.ApplicationIdentifier);
                    table.ForeignKey(
                        name: "FK_Applications_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Applications_Candidates_CandidateIdentifier",
                        column: x => x.CandidateIdentifier,
                        principalTable: "Candidates",
                        principalColumn: "CandidateIdentifier",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applications_Vacancies_VacancyId",
                        column: x => x.VacancyId,
                        principalTable: "Vacancies",
                        principalColumn: "VacancyIdentifier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vacancies_EnterpriseIdentifier",
                table: "Vacancies",
                column: "EnterpriseIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_CandidateId",
                table: "Applications",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_CandidateIdentifier",
                table: "Applications",
                column: "CandidateIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_VacancyId_CandidateIdentifier",
                table: "Applications",
                columns: new[] { "VacancyId", "CandidateIdentifier" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Vacancies_Enterprises_EnterpriseIdentifier",
                table: "Vacancies",
                column: "EnterpriseIdentifier",
                principalTable: "Enterprises",
                principalColumn: "EnterpriseIdentifier",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vacancies_Enterprises_EnterpriseIdentifier",
                table: "Vacancies");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Vacancies_EnterpriseIdentifier",
                table: "Vacancies");
        }
    }
}
