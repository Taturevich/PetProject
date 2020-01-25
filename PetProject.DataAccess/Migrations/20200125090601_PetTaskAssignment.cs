using Microsoft.EntityFrameworkCore.Migrations;

namespace PetProject.DataAccess.Migrations
{
    public partial class PetTaskAssignment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PetTaskAssignments",
                columns: table => new
                {
                    PetTaskAssignmentId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PetId = table.Column<int>(nullable: false),
                    TaskTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetTaskAssignments", x => x.PetTaskAssignmentId);
                    table.ForeignKey(
                        name: "FK_PetTaskAssignments_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "PetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PetTaskAssignments_TaskTypes_TaskTypeId",
                        column: x => x.TaskTypeId,
                        principalTable: "TaskTypes",
                        principalColumn: "TaskTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PetTaskAssignments_PetId",
                table: "PetTaskAssignments",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_PetTaskAssignments_TaskTypeId",
                table: "PetTaskAssignments",
                column: "TaskTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PetTaskAssignments");
        }
    }
}
