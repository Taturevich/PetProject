using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PetProject.DataAccess.Migrations
{
    public partial class Initial : Migration
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
                name: "TaskTypes",
                columns: table => new
                {
                    TaskTypeId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    PetPoints = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    DefaultDuration = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTypes", x => x.TaskTypeId);
                });

            migrationBuilder.CreateTable(
                name: "UserFeature",
                columns: table => new
                {
                    UserFeatureId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Category = table.Column<string>(nullable: true),
                    Characteristic = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFeature", x => x.UserFeatureId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Role = table.Column<int>(nullable: false),
                    PetPoints = table.Column<int>(nullable: false),
                    IsBlackListed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Pets",
                columns: table => new
                {
                    PetId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    VolunteerId = table.Column<int>(nullable: false),
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
                name: "UserFeatureAssignments",
                columns: table => new
                {
                    UserFeatureAssignmentId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(nullable: false),
                    UserFeatureId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFeatureAssignments", x => x.UserFeatureAssignmentId);
                    table.ForeignKey(
                        name: "FK_UserFeatureAssignments_UserFeature_UserFeatureId",
                        column: x => x.UserFeatureId,
                        principalTable: "UserFeature",
                        principalColumn: "UserFeatureId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFeatureAssignments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSocialNetworks",
                columns: table => new
                {
                    UserSocialNetworkId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NetworkType = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSocialNetworks", x => x.UserSocialNetworkId);
                    table.ForeignKey(
                        name: "FK_UserSocialNetworks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
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

            migrationBuilder.CreateTable(
                name: "PetTaskTypeAssignments",
                columns: table => new
                {
                    PetTaskTypeAssignmentId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PetId = table.Column<int>(nullable: false),
                    TaskTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetTaskTypeAssignments", x => x.PetTaskTypeAssignmentId);
                    table.ForeignKey(
                        name: "FK_PetTaskTypeAssignments_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "PetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PetTaskTypeAssignments_TaskTypes_TaskTypeId",
                        column: x => x.TaskTypeId,
                        principalTable: "TaskTypes",
                        principalColumn: "TaskTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    TaskId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Status = table.Column<string>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    TaskTypeId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    PetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.TaskId);
                    table.ForeignKey(
                        name: "FK_Tasks_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "PetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_TaskTypes_TaskTypeId",
                        column: x => x.TaskTypeId,
                        principalTable: "TaskTypes",
                        principalColumn: "TaskTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
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

            migrationBuilder.CreateIndex(
                name: "IX_PetTaskTypeAssignments_PetId",
                table: "PetTaskTypeAssignments",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_PetTaskTypeAssignments_TaskTypeId",
                table: "PetTaskTypeAssignments",
                column: "TaskTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_PetId",
                table: "Tasks",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskTypeId",
                table: "Tasks",
                column: "TaskTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_UserId",
                table: "Tasks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFeatureAssignments_UserFeatureId",
                table: "UserFeatureAssignments",
                column: "UserFeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFeatureAssignments_UserId",
                table: "UserFeatureAssignments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSocialNetworks_UserId",
                table: "UserSocialNetworks",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PetFeatureAssignments");

            migrationBuilder.DropTable(
                name: "PetTaskTypeAssignments");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "UserFeatureAssignments");

            migrationBuilder.DropTable(
                name: "UserSocialNetworks");

            migrationBuilder.DropTable(
                name: "PetFeatures");

            migrationBuilder.DropTable(
                name: "Pets");

            migrationBuilder.DropTable(
                name: "TaskTypes");

            migrationBuilder.DropTable(
                name: "UserFeature");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "PetStatuses");
        }
    }
}
