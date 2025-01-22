using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedMissingAdminUser_MyOversightFrom141 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "cb6397fe-acf8-49dd-b791-01bf0b069aee", 0, "adb2ad4efab64d049c8a713991f0bd37", new DateOnly(1990, 7, 1), "admin@localhost.com", true, "Admin", "Default", false, null, "ADMIN@LOCALHOST.COM", "ADMIN@LOCALHOST.COM", "AQAAAAIAAYagAAAAEN5WCPZ+e5Tcc6puplTNrflD+R6WpF82fsT2aCWMlDDmwAlhys5FMsfFVgax4+GI7Q==", null, false, "26d82787-3f4c-4fe2-b6c6-1660a3c8d58a", false, "admin@localhost.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cb6397fe-acf8-49dd-b791-01bf0b069aee");
        }
    }
}
