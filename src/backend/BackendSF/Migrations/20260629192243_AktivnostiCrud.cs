using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendSF.Migrations
{
    /// <inheritdoc />
    public partial class AktivnostiCrud : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_PlanoviPutovanja_PlanPutovanjaId",
                table: "Activities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activities",
                table: "Activities");

            migrationBuilder.RenameTable(
                name: "Activities",
                newName: "Aktivnosti");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Aktivnosti",
                newName: "Naziv");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Aktivnosti",
                newName: "Datum");

            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Aktivnosti",
                newName: "Vrijeme");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Aktivnosti",
                newName: "Lokacija");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Aktivnosti",
                newName: "Opis");

            migrationBuilder.RenameColumn(
                name: "EstimatedCost",
                table: "Aktivnosti",
                newName: "ProcijenjeniTrosak");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_PlanPutovanjaId",
                table: "Aktivnosti",
                newName: "IX_Aktivnosti_PlanPutovanjaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Aktivnosti",
                table: "Aktivnosti",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Aktivnosti_PlanoviPutovanja_PlanPutovanjaId",
                table: "Aktivnosti",
                column: "PlanPutovanjaId",
                principalTable: "PlanoviPutovanja",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aktivnosti_PlanoviPutovanja_PlanPutovanjaId",
                table: "Aktivnosti");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Aktivnosti",
                table: "Aktivnosti");

            migrationBuilder.RenameColumn(
                name: "Naziv",
                table: "Aktivnosti",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Datum",
                table: "Aktivnosti",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "Vrijeme",
                table: "Aktivnosti",
                newName: "Time");

            migrationBuilder.RenameColumn(
                name: "Lokacija",
                table: "Aktivnosti",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "Opis",
                table: "Aktivnosti",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "ProcijenjeniTrosak",
                table: "Aktivnosti",
                newName: "EstimatedCost");

            migrationBuilder.RenameTable(
                name: "Aktivnosti",
                newName: "Activities");

            migrationBuilder.RenameIndex(
                name: "IX_Aktivnosti_PlanPutovanjaId",
                table: "Activities",
                newName: "IX_Activities_PlanPutovanjaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activities",
                table: "Activities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_PlanoviPutovanja_PlanPutovanjaId",
                table: "Activities",
                column: "PlanPutovanjaId",
                principalTable: "PlanoviPutovanja",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
