using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiEmpresas.Migrations
{
    /// <inheritdoc />
    public partial class CreateFuncionarioSetor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profissoes_Setores_SetorId",
                table: "Profissoes");

            migrationBuilder.AlterColumn<Guid>(
                name: "SetorId",
                table: "Profissoes",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "FuncionarioSetor",
                columns: table => new
                {
                    FuncionarioId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SetorId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuncionarioSetor", x => new { x.FuncionarioId, x.SetorId });
                    table.ForeignKey(
                        name: "FK_FuncionarioSetor_Funcionarios_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "Funcionarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FuncionarioSetor_Setores_SetorId",
                        column: x => x.SetorId,
                        principalTable: "Setores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_FuncionarioSetor_SetorId",
                table: "FuncionarioSetor",
                column: "SetorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Profissoes_Setores_SetorId",
                table: "Profissoes",
                column: "SetorId",
                principalTable: "Setores",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profissoes_Setores_SetorId",
                table: "Profissoes");

            migrationBuilder.DropTable(
                name: "FuncionarioSetor");

            migrationBuilder.AlterColumn<Guid>(
                name: "SetorId",
                table: "Profissoes",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddForeignKey(
                name: "FK_Profissoes_Setores_SetorId",
                table: "Profissoes",
                column: "SetorId",
                principalTable: "Setores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
