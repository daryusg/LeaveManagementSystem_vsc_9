using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeaveManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDefaultRolesandUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2f26c8ac-3971-40df-bcce-ee90609328c6", null, "Supervisor", "SUPERVISOR" },
                    { "39c75408-f38a-4bec-b52a-9aa5fcee5bc8", null, "Employee", "EMPLOYEE" },
                    { "f9080104-d003-43fe-b7b8-91c02c6bacd2", null, "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "8a862852-9e68-4bcc-b624-220e9b060cf9", 0, "bff9869ba62a4a4986479a91c0d6890b", "admin_bu1@localhost.com", true, false, null, "ADMIN_BU1@LOCALHOST.COM", "ADMIN_BU1@LOCALHOST.COM", "AQAAAAIAAYagAAAAEJ5jT7bufJvPmguGXW9QAbwUO4GGPlOPUpZDSdL3J5uk9pgBbth4gkSONtFF3+A8kg==", null, false, "8dd5bb17-2a8e-4998-a66b-8611e32a9b9a", false, "admin_bu1@localhost.com" },
                    { "a23d75b8-c842-4164-9cb1-f9e7c2366c3b", 0, "939075471b23410b832121d4f56ebeb3", "testuser@leavemanagement.com", true, false, null, "TESTUSER@LEAVEMANAGEMENT.COM", "TESTUSER@LEAVEMANAGEMENT.COM", "AQAAAAIAAYagAAAAEFmo3ccUYE1zfv2SyMqUSjAxLxupY6bcQEuHLVIgF+ShU/lOkD2lbsPYrlcXNnKNnQ==", null, false, "49123653-2d14-4579-a213-e8263f280755", false, "testuser@leavemanagement.com" },
                    { "cb6397fe-acf8-49dd-b791-01bf0b069aee", 0, "adb2ad4efab64d049c8a713991f0bd37", "admin@localhost.com", true, false, null, "ADMIN@LOCALHOST.COM", "ADMIN@LOCALHOST.COM", "AQAAAAIAAYagAAAAEN5WCPZ+e5Tcc6puplTNrflD+R6WpF82fsT2aCWMlDDmwAlhys5FMsfFVgax4+GI7Q==", null, false, "26d82787-3f4c-4fe2-b6c6-1660a3c8d58a", false, "admin@localhost.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "f9080104-d003-43fe-b7b8-91c02c6bacd2", "8a862852-9e68-4bcc-b624-220e9b060cf9" },
                    { "39c75408-f38a-4bec-b52a-9aa5fcee5bc8", "a23d75b8-c842-4164-9cb1-f9e7c2366c3b" },
                    { "f9080104-d003-43fe-b7b8-91c02c6bacd2", "cb6397fe-acf8-49dd-b791-01bf0b069aee" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2f26c8ac-3971-40df-bcce-ee90609328c6");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "f9080104-d003-43fe-b7b8-91c02c6bacd2", "8a862852-9e68-4bcc-b624-220e9b060cf9" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "39c75408-f38a-4bec-b52a-9aa5fcee5bc8", "a23d75b8-c842-4164-9cb1-f9e7c2366c3b" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "f9080104-d003-43fe-b7b8-91c02c6bacd2", "cb6397fe-acf8-49dd-b791-01bf0b069aee" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "39c75408-f38a-4bec-b52a-9aa5fcee5bc8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f9080104-d003-43fe-b7b8-91c02c6bacd2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8a862852-9e68-4bcc-b624-220e9b060cf9");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a23d75b8-c842-4164-9cb1-f9e7c2366c3b");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cb6397fe-acf8-49dd-b791-01bf0b069aee");
        }
    }
}
