using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class Retry_AddToAspNetUserRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //NOTE: 21/01/25 this was empty so, as advised by chatgpt, i added it manually.
            // Insert user-role assignments (assuming role and user IDs are correct)
            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "cb6397fe-acf8-49dd-b791-01bf0b069aee", "f9080104-d003-43fe-b7b8-91c02c6bacd2" }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //NOTE: 21/01/25 this was empty so, as advised by chatgpt, i added it manually.
            // Delete the user-role assignments (reverse of Up)
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "cb6397fe-acf8-49dd-b791-01bf0b069aee", "f9080104-d003-43fe-b7b8-91c02c6bacd2" }
            );
        }
    }
}
