using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiEmpresas.Migrations
{
    /// <inheritdoc />
    public partial class RemovidoProfissao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Funcionarios_Profissoes_ProfissaoId",
                table: "Funcionarios");

            migrationBuilder.DropTable(
                name: "Profissoes");

            migrationBuilder.DropIndex(
                name: "IX_Funcionarios_ProfissaoId",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "ProfissaoId",
                table: "Funcionarios");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProfissaoId",
                table: "Funcionarios",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "Profissoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nome = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SetorId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profissoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profissoes_Setores_SetorId",
                        column: x => x.SetorId,
                        principalTable: "Setores",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_ProfissaoId",
                table: "Funcionarios",
                column: "ProfissaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Profissoes_SetorId",
                table: "Profissoes",
                column: "SetorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Funcionarios_Profissoes_ProfissaoId",
                table: "Funcionarios",
                column: "ProfissaoId",
                principalTable: "Profissoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
