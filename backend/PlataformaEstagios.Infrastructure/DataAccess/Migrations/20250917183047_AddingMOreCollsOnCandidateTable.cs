using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlataformaEstagios.Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddingMOreCollsOnCandidateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePicturePath",
                table: "Candidates",
                type: "character varying(260)",
                maxLength: 260,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResumePath",
                table: "Candidates",
                type: "character varying(260)",
                maxLength: 260,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePicturePath",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "ResumePath",
                table: "Candidates");
        }
    }
}
