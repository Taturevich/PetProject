using Microsoft.EntityFrameworkCore.Migrations;

namespace PetProject.DataAccess.Migrations
{
    public partial class AddPets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PetFeatures",
                columns: table => new
                {
                    PetFeatureId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Category = table.Column<string>(nullable: true),
                    Characteristic = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetFeatures", x => x.PetFeatureId);
                });

            migrationBuilder.CreateTable(
                name: "PetStatuses",
                columns: table => new
                {
                    PetStatusId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetStatuses", x => x.PetStatusId);
                });

            migrationBuilder.CreateTable(
                name: "Pets",
                columns: table => new
                {
                    PetId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Volunteer = table.Column<string>(nullable: true),
                    PetStatusId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pets", x => x.PetId);
                    table.ForeignKey(
                        name: "FK_Pets_PetStatuses_PetStatusId",
                        column: x => x.PetStatusId,
                        principalTable: "PetStatuses",
                        principalColumn: "PetStatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PetFeatureAssignments",
                columns: table => new
                {
                    PetFeatureAssignmentId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PetFeatureId = table.Column<int>(nullable: false),
                    PetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetFeatureAssignments", x => x.PetFeatureAssignmentId);
                    table.ForeignKey(
                        name: "FK_PetFeatureAssignments_PetFeatures_PetFeatureId",
                        column: x => x.PetFeatureId,
                        principalTable: "PetFeatures",
                        principalColumn: "PetFeatureId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PetFeatureAssignments_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "PetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PetFeatureAssignments_PetFeatureId",
                table: "PetFeatureAssignments",
                column: "PetFeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_PetFeatureAssignments_PetId",
                table: "PetFeatureAssignments",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_PetStatusId",
                table: "Pets",
                column: "PetStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PetFeatureAssignments");

            migrationBuilder.DropTable(
                name: "PetFeatures");

            migrationBuilder.DropTable(
                name: "Pets");

            migrationBuilder.DropTable(
                name: "PetStatuses");
        }
    }
}
