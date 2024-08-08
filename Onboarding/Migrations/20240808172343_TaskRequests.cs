using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Onboarding.Migrations
{
    /// <inheritdoc />
    public partial class TaskRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorklowIstanceId",
                table: "TaskRequests",
                newName: "WorklowRequestId");

            migrationBuilder.AddColumn<Guid>(
                name: "WorkflowRequestId",
                table: "TaskRequests",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TaskRequests_WorkflowRequestId",
                table: "TaskRequests",
                column: "WorkflowRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskRequests_WorkflowRequests_WorkflowRequestId",
                table: "TaskRequests",
                column: "WorkflowRequestId",
                principalTable: "WorkflowRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskRequests_WorkflowRequests_WorkflowRequestId",
                table: "TaskRequests");

            migrationBuilder.DropIndex(
                name: "IX_TaskRequests_WorkflowRequestId",
                table: "TaskRequests");

            migrationBuilder.DropColumn(
                name: "WorkflowRequestId",
                table: "TaskRequests");

            migrationBuilder.RenameColumn(
                name: "WorklowRequestId",
                table: "TaskRequests",
                newName: "WorklowIstanceId");
        }
    }
}
