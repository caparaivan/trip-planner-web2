using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendSF.Migrations
{
    /// <inheritdoc />
    public partial class DestinacijeCrud : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Destinations_PlanoviPutovanja_PlanPutovanjaId",
                table: "Destinations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Destinations",
                table: "Destinations");

            migrationBuilder.RenameTable(
                name: "Destinations",
                newName: "Destinacije");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Destinacije",
                newName: "Naziv");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Destinacije",
                newName: "Lokacija");

            migrationBuilder.RenameColumn(
                name: "ArrivalDate",
                table: "Destinacije",
                newName: "DatumDolaska");

            migrationBuilder.RenameColumn(
                name: "DepartureDate",
                table: "Destinacije",
                newName: "DatumOdlaska");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Destinacije",
                newName: "KratakOpis");

            migrationBuilder.RenameIndex(
                name: "IX_Destinations_PlanPutovanjaId",
                table: "Destinacije",
                newName: "IX_Destinacije_PlanPutovanjaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Destinacije",
                table: "Destinacije",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Destinacije_PlanoviPutovanja_PlanPutovanjaId",
                table: "Destinacije",
                column: "PlanPutovanjaId",
                principalTable: "PlanoviPutovanja",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Destinacije_PlanoviPutovanja_PlanPutovanjaId",
                table: "Destinacije");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Destinacije",
                table: "Destinacije");

            migrationBuilder.RenameColumn(
                name: "Naziv",
                table: "Destinacije",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Lokacija",
                table: "Destinacije",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "DatumDolaska",
                table: "Destinacije",
                newName: "ArrivalDate");

            migrationBuilder.RenameColumn(
                name: "DatumOdlaska",
                table: "Destinacije",
                newName: "DepartureDate");

            migrationBuilder.RenameColumn(
                name: "KratakOpis",
                table: "Destinacije",
                newName: "Description");

            migrationBuilder.RenameTable(
                name: "Destinacije",
                newName: "Destinations");

            migrationBuilder.RenameIndex(
                name: "IX_Destinacije_PlanPutovanjaId",
                table: "Destinations",
                newName: "IX_Destinations_PlanPutovanjaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Destinations",
                table: "Destinations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Destinations_PlanoviPutovanja_PlanPutovanjaId",
                table: "Destinations",
                column: "PlanPutovanjaId",
                principalTable: "PlanoviPutovanja",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}