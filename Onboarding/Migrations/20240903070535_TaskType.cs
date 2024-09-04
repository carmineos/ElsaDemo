using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Onboarding.Migrations
{
    /// <inheritdoc />
    public partial class TaskType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "TaskRequests");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "TaskRequests");

            migrationBuilder.AddColumn<int>(
                name: "TaskTypeId",
                table: "TaskRequests",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TaskType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskRequests_TaskTypeId",
                table: "TaskRequests",
                column: "TaskTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskRequests_TaskType_TaskTypeId",
                table: "TaskRequests",
                column: "TaskTypeId",
                principalTable: "TaskType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskRequests_TaskType_TaskTypeId",
                table: "TaskRequests");

            migrationBuilder.DropTable(
                name: "TaskType");

            migrationBuilder.DropIndex(
                name: "IX_TaskRequests_TaskTypeId",
                table: "TaskRequests");

            migrationBuilder.DropColumn(
                name: "TaskTypeId",
                table: "TaskRequests");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TaskRequests",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TaskRequests",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
