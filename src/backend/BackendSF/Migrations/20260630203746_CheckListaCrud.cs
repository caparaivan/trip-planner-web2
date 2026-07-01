using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendSF.Migrations
{
    /// <inheritdoc />
    public partial class CheckListaCrud : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistItems_PlanoviPutovanja_PlanPutovanjaId",
                table: "ChecklistItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChecklistItems",
                table: "ChecklistItems");

            migrationBuilder.RenameTable(
                name: "ChecklistItems",
                newName: "StavkeCheckListe");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "StavkeCheckListe",
                newName: "Naziv");

            migrationBuilder.RenameColumn(
                name: "IsDone",
                table: "StavkeCheckListe",
                newName: "Zavrseno");

            migrationBuilder.RenameIndex(
                name: "IX_ChecklistItems_PlanPutovanjaId",
                table: "StavkeCheckListe",
                newName: "IX_StavkeCheckListe_PlanPutovanjaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StavkeCheckListe",
                table: "StavkeCheckListe",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StavkeCheckListe_PlanoviPutovanja_PlanPutovanjaId",
                table: "StavkeCheckListe",
                column: "PlanPutovanjaId",
                principalTable: "PlanoviPutovanja",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StavkeCheckListe_PlanoviPutovanja_PlanPutovanjaId",
                table: "StavkeCheckListe");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StavkeCheckListe",
                table: "StavkeCheckListe");

            migrationBuilder.RenameTable(
                name: "StavkeCheckListe",
                newName: "ChecklistItems");

            migrationBuilder.RenameColumn(
                name: "Naziv",
                table: "ChecklistItems",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Zavrseno",
                table: "ChecklistItems",
                newName: "IsDone");

            migrationBuilder.RenameIndex(
                name: "IX_StavkeCheckListe_PlanPutovanjaId",
                table: "ChecklistItems",
                newName: "IX_ChecklistItems_PlanPutovanjaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChecklistItems",
                table: "ChecklistItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistItems_PlanoviPutovanja_PlanPutovanjaId",
                table: "ChecklistItems",
                column: "PlanPutovanjaId",
                principalTable: "PlanoviPutovanja",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
