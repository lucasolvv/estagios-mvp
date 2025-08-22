using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlataformaEstagios.Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCnpjToMaxLength18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Cnpj",
                table: "Enterprises",
                type: "character varying(18)",
                maxLength: 18,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(14)",
                oldMaxLength: 14,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Cnpj",
                table: "Enterprises",
                type: "character varying(14)",
                maxLength: 14,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(18)",
                oldMaxLength: 18,
                oldNullable: true);
        }
    }
}
