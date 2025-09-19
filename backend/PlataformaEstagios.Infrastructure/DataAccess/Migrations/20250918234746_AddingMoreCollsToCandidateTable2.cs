using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlataformaEstagios.Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddingMoreCollsToCandidateTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BioResume",
                table: "Candidates",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BioResume",
                table: "Candidates");
        }
    }
}
