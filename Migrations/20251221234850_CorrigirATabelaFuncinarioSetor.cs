using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiEmpresas.Migrations
{
    /// <inheritdoc />
    public partial class CorrigirATabelaFuncinarioSetor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Funcionarios_Empresas_EmpresaId",
                table: "Funcionarios");

            migrationBuilder.DropForeignKey(
                name: "FK_FuncionarioSetor_Funcionarios_FuncionarioId",
                table: "FuncionarioSetor");

            migrationBuilder.DropForeignKey(
                name: "FK_FuncionarioSetor_Setores_SetorId",
                table: "FuncionarioSetor");

            migrationBuilder.DropIndex(
                name: "IX_Funcionarios_EmpresaId",
                table: "Funcionarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FuncionarioSetor",
                table: "FuncionarioSetor");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Funcionarios");

            migrationBuilder.RenameTable(
                name: "FuncionarioSetor",
                newName: "FuncionarioSetores");

            migrationBuilder.RenameIndex(
                name: "IX_FuncionarioSetor_SetorId",
                table: "FuncionarioSetores",
                newName: "IX_FuncionarioSetores_SetorId");

            migrationBuilder.AddColumn<Guid>(
                name: "EmpresaId",
                table: "FuncionarioSetores",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<decimal>(
                name: "Salario",
                table: "FuncionarioSetores",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FuncionarioSetores",
                table: "FuncionarioSetores",
                columns: new[] { "FuncionarioId", "EmpresaId", "SetorId" });

            migrationBuilder.CreateIndex(
                name: "IX_FuncionarioSetores_EmpresaId_SetorId",
                table: "FuncionarioSetores",
                columns: new[] { "EmpresaId", "SetorId" });

            migrationBuilder.AddForeignKey(
                name: "FK_FuncionarioSetores_EmpresaSetores_EmpresaId_SetorId",
                table: "FuncionarioSetores",
                columns: new[] { "EmpresaId", "SetorId" },
                principalTable: "EmpresaSetores",
                principalColumns: new[] { "EmpresaId", "SetorId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FuncionarioSetores_Empresas_EmpresaId",
                table: "FuncionarioSetores",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FuncionarioSetores_Funcionarios_FuncionarioId",
                table: "FuncionarioSetores",
                column: "FuncionarioId",
                principalTable: "Funcionarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FuncionarioSetores_Setores_SetorId",
                table: "FuncionarioSetores",
                column: "SetorId",
                principalTable: "Setores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuncionarioSetores_EmpresaSetores_EmpresaId_SetorId",
                table: "FuncionarioSetores");

            migrationBuilder.DropForeignKey(
                name: "FK_FuncionarioSetores_Empresas_EmpresaId",
                table: "FuncionarioSetores");

            migrationBuilder.DropForeignKey(
                name: "FK_FuncionarioSetores_Funcionarios_FuncionarioId",
                table: "FuncionarioSetores");

            migrationBuilder.DropForeignKey(
                name: "FK_FuncionarioSetores_Setores_SetorId",
                table: "FuncionarioSetores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FuncionarioSetores",
                table: "FuncionarioSetores");

            migrationBuilder.DropIndex(
                name: "IX_FuncionarioSetores_EmpresaId_SetorId",
                table: "FuncionarioSetores");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "FuncionarioSetores");

            migrationBuilder.DropColumn(
                name: "Salario",
                table: "FuncionarioSetores");

            migrationBuilder.RenameTable(
                name: "FuncionarioSetores",
                newName: "FuncionarioSetor");

            migrationBuilder.RenameIndex(
                name: "IX_FuncionarioSetores_SetorId",
                table: "FuncionarioSetor",
                newName: "IX_FuncionarioSetor_SetorId");

            migrationBuilder.AddColumn<Guid>(
                name: "EmpresaId",
                table: "Funcionarios",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FuncionarioSetor",
                table: "FuncionarioSetor",
                columns: new[] { "FuncionarioId", "SetorId" });

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_EmpresaId",
                table: "Funcionarios",
                column: "EmpresaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Funcionarios_Empresas_EmpresaId",
                table: "Funcionarios",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FuncionarioSetor_Funcionarios_FuncionarioId",
                table: "FuncionarioSetor",
                column: "FuncionarioId",
                principalTable: "Funcionarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FuncionarioSetor_Setores_SetorId",
                table: "FuncionarioSetor",
                column: "SetorId",
                principalTable: "Setores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
