using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PlataformaEstagios.Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserIdentifier = table.Column<Guid>(type: "uuid", nullable: false),
                    Nickname = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    UserType = table.Column<int>(type: "integer", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.UniqueConstraint("AK_Users_UserIdentifier", x => x.UserIdentifier);
                });

            migrationBuilder.CreateTable(
                name: "Candidates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserIdentifier = table.Column<Guid>(type: "uuid", nullable: false),
                    CandidateIdentifier = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CourseName = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidates", x => x.Id);
                    table.UniqueConstraint("AK_Candidates_CandidateIdentifier", x => x.CandidateIdentifier);
                    table.ForeignKey(
                        name: "FK_Candidates_Users_UserIdentifier",
                        column: x => x.UserIdentifier,
                        principalTable: "Users",
                        principalColumn: "UserIdentifier",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Enterprises",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EnterpriseIdentifier = table.Column<Guid>(type: "uuid", nullable: false),
                    UserIdentifier = table.Column<Guid>(type: "uuid", nullable: false),
                    EnterpriseName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Cnpj = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: true),
                    ActivityArea = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enterprises", x => x.Id);
                    table.UniqueConstraint("AK_Enterprises_EnterpriseIdentifier", x => x.EnterpriseIdentifier);
                    table.ForeignKey(
                        name: "FK_Enterprises_Users_UserIdentifier",
                        column: x => x.UserIdentifier,
                        principalTable: "Users",
                        principalColumn: "UserIdentifier",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AddressIdentifier = table.Column<Guid>(type: "uuid", nullable: false),
                    Street = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    Complement = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    Neighborhood = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    City = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    UF = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    CEP = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: true),
                    CandidateIdentifier = table.Column<Guid>(type: "uuid", nullable: true),
                    EnterpriseIdentifier = table.Column<Guid>(type: "uuid", nullable: true),
                    Active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.CheckConstraint("CK_Addresses_Owner", "(\"CandidateIdentifier\" IS NOT NULL AND \"EnterpriseIdentifier\" IS NULL) OR (\"CandidateIdentifier\" IS NULL AND \"EnterpriseIdentifier\" IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_Addresses_Candidates_CandidateIdentifier",
                        column: x => x.CandidateIdentifier,
                        principalTable: "Candidates",
                        principalColumn: "CandidateIdentifier",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Addresses_Enterprises_EnterpriseIdentifier",
                        column: x => x.EnterpriseIdentifier,
                        principalTable: "Enterprises",
                        principalColumn: "EnterpriseIdentifier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CandidateIdentifier",
                table: "Addresses",
                column: "CandidateIdentifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_EnterpriseIdentifier",
                table: "Addresses",
                column: "EnterpriseIdentifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_UserIdentifier",
                table: "Candidates",
                column: "UserIdentifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enterprises_UserIdentifier",
                table: "Enterprises",
                column: "UserIdentifier",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Candidates");

            migrationBuilder.DropTable(
                name: "Enterprises");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
