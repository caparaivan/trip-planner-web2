using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendSF.Migrations
{
    /// <inheritdoc />
    public partial class PlanPutovanjaCrud : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_TripPlans_TripPlanId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistItems_TripPlans_TripPlanId",
                table: "ChecklistItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Destinations_TripPlans_TripPlanId",
                table: "Destinations");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_TripPlans_TripPlanId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_TripPlans_Users_UserId",
                table: "TripPlans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TripPlans",
                table: "TripPlans");

            migrationBuilder.RenameTable(
                name: "TripPlans",
                newName: "PlanoviPutovanja");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "PlanoviPutovanja",
                newName: "Naziv");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "PlanoviPutovanja",
                newName: "KratakOpis");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "PlanoviPutovanja",
                newName: "PocetniDatum");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "PlanoviPutovanja",
                newName: "KrajnjiDatum");

            migrationBuilder.RenameColumn(
                name: "PlannedBudget",
                table: "PlanoviPutovanja",
                newName: "PlaniraniBudzet");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "PlanoviPutovanja",
                newName: "OpsteNapomene");

            migrationBuilder.RenameColumn(
                name: "CreatedAtUtc",
                table: "PlanoviPutovanja",
                newName: "DatumKreiranjaUtc");

            migrationBuilder.RenameIndex(
                name: "IX_TripPlans_UserId",
                table: "PlanoviPutovanja",
                newName: "IX_PlanoviPutovanja_UserId");

            migrationBuilder.RenameColumn(
                name: "TripPlanId",
                table: "Expenses",
                newName: "PlanPutovanjaId");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_TripPlanId",
                table: "Expenses",
                newName: "IX_Expenses_PlanPutovanjaId");

            migrationBuilder.RenameColumn(
                name: "TripPlanId",
                table: "Destinations",
                newName: "PlanPutovanjaId");

            migrationBuilder.RenameIndex(
                name: "IX_Destinations_TripPlanId",
                table: "Destinations",
                newName: "IX_Destinations_PlanPutovanjaId");

            migrationBuilder.RenameColumn(
                name: "TripPlanId",
                table: "ChecklistItems",
                newName: "PlanPutovanjaId");

            migrationBuilder.RenameIndex(
                name: "IX_ChecklistItems_TripPlanId",
                table: "ChecklistItems",
                newName: "IX_ChecklistItems_PlanPutovanjaId");

            migrationBuilder.RenameColumn(
                name: "TripPlanId",
                table: "Activities",
                newName: "PlanPutovanjaId");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_TripPlanId",
                table: "Activities",
                newName: "IX_Activities_PlanPutovanjaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlanoviPutovanja",
                table: "PlanoviPutovanja",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanoviPutovanja_Users_UserId",
                table: "PlanoviPutovanja",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_PlanoviPutovanja_PlanPutovanjaId",
                table: "Activities",
                column: "PlanPutovanjaId",
                principalTable: "PlanoviPutovanja",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistItems_PlanoviPutovanja_PlanPutovanjaId",
                table: "ChecklistItems",
                column: "PlanPutovanjaId",
                principalTable: "PlanoviPutovanja",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Destinations_PlanoviPutovanja_PlanPutovanjaId",
                table: "Destinations",
                column: "PlanPutovanjaId",
                principalTable: "PlanoviPutovanja",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_PlanoviPutovanja_PlanPutovanjaId",
                table: "Expenses",
                column: "PlanPutovanjaId",
                principalTable: "PlanoviPutovanja",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_PlanoviPutovanja_PlanPutovanjaId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_ChecklistItems_PlanoviPutovanja_PlanPutovanjaId",
                table: "ChecklistItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Destinations_PlanoviPutovanja_PlanPutovanjaId",
                table: "Destinations");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_PlanoviPutovanja_PlanPutovanjaId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanoviPutovanja_Users_UserId",
                table: "PlanoviPutovanja");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlanoviPutovanja",
                table: "PlanoviPutovanja");

            migrationBuilder.RenameColumn(
                name: "PlanPutovanjaId",
                table: "Expenses",
                newName: "TripPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_PlanPutovanjaId",
                table: "Expenses",
                newName: "IX_Expenses_TripPlanId");

            migrationBuilder.RenameColumn(
                name: "PlanPutovanjaId",
                table: "Destinations",
                newName: "TripPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_Destinations_PlanPutovanjaId",
                table: "Destinations",
                newName: "IX_Destinations_TripPlanId");

            migrationBuilder.RenameColumn(
                name: "PlanPutovanjaId",
                table: "ChecklistItems",
                newName: "TripPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_ChecklistItems_PlanPutovanjaId",
                table: "ChecklistItems",
                newName: "IX_ChecklistItems_TripPlanId");

            migrationBuilder.RenameColumn(
                name: "PlanPutovanjaId",
                table: "Activities",
                newName: "TripPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_PlanPutovanjaId",
                table: "Activities",
                newName: "IX_Activities_TripPlanId");

            migrationBuilder.RenameColumn(
                name: "Naziv",
                table: "PlanoviPutovanja",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "KratakOpis",
                table: "PlanoviPutovanja",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "PocetniDatum",
                table: "PlanoviPutovanja",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "KrajnjiDatum",
                table: "PlanoviPutovanja",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "PlaniraniBudzet",
                table: "PlanoviPutovanja",
                newName: "PlannedBudget");

            migrationBuilder.RenameColumn(
                name: "OpsteNapomene",
                table: "PlanoviPutovanja",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "DatumKreiranjaUtc",
                table: "PlanoviPutovanja",
                newName: "CreatedAtUtc");

            migrationBuilder.RenameTable(
                name: "PlanoviPutovanja",
                newName: "TripPlans");

            migrationBuilder.RenameIndex(
                name: "IX_PlanoviPutovanja_UserId",
                table: "TripPlans",
                newName: "IX_TripPlans_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TripPlans",
                table: "TripPlans",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TripPlans_Users_UserId",
                table: "TripPlans",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_TripPlans_TripPlanId",
                table: "Activities",
                column: "TripPlanId",
                principalTable: "TripPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChecklistItems_TripPlans_TripPlanId",
                table: "ChecklistItems",
                column: "TripPlanId",
                principalTable: "TripPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Destinations_TripPlans_TripPlanId",
                table: "Destinations",
                column: "TripPlanId",
                principalTable: "TripPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_TripPlans_TripPlanId",
                table: "Expenses",
                column: "TripPlanId",
                principalTable: "TripPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
