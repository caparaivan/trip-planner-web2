using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendSF.Migrations
{
    /// <inheritdoc />
    public partial class TroskoviBudzetCrud : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_PlanoviPutovanja_PlanPutovanjaId",
                table: "Expenses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Expenses",
                table: "Expenses");

            migrationBuilder.RenameTable(
                name: "Expenses",
                newName: "Troskovi");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Troskovi",
                newName: "Naziv");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Troskovi",
                newName: "Kategorija");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Troskovi",
                newName: "Iznos");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Troskovi",
                newName: "Datum");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Troskovi",
                newName: "Opis");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_PlanPutovanjaId",
                table: "Troskovi",
                newName: "IX_Troskovi_PlanPutovanjaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Troskovi",
                table: "Troskovi",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Troskovi_PlanoviPutovanja_PlanPutovanjaId",
                table: "Troskovi",
                column: "PlanPutovanjaId",
                principalTable: "PlanoviPutovanja",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Troskovi_PlanoviPutovanja_PlanPutovanjaId",
                table: "Troskovi");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Troskovi",
                table: "Troskovi");

            migrationBuilder.RenameColumn(
                name: "Naziv",
                table: "Troskovi",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Kategorija",
                table: "Troskovi",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "Iznos",
                table: "Troskovi",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "Datum",
                table: "Troskovi",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "Opis",
                table: "Troskovi",
                newName: "Description");

            migrationBuilder.RenameTable(
                name: "Troskovi",
                newName: "Expenses");

            migrationBuilder.RenameIndex(
                name: "IX_Troskovi_PlanPutovanjaId",
                table: "Expenses",
                newName: "IX_Expenses_PlanPutovanjaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Expenses",
                table: "Expenses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_PlanoviPutovanja_PlanPutovanjaId",
                table: "Expenses",
                column: "PlanPutovanjaId",
                principalTable: "PlanoviPutovanja",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
