using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedApplicationUserConfigData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8a862852-9e68-4bcc-b624-220e9b060cf9",
                columns: new[] { "DateOfBirth", "Email", "FirstName", "LastName", "NormalizedEmail", "NormalizedUserName", "UserName" },
                values: new object[] { new DateOnly(1985, 7, 11), "supervisor@daryus.com", "Supervisor", "Anonymous", "SUPERVISOR@DARYUS.COM", "SUPERVISOR@DARYUS.COM", "supervisor@daryus.com" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a23d75b8-c842-4164-9cb1-f9e7c2366c3b",
                columns: new[] { "DateOfBirth", "Email", "FirstName", "LastName", "NormalizedEmail", "NormalizedUserName", "UserName" },
                values: new object[] { new DateOnly(1992, 11, 7), "employee@daryus.com", "Employee", "Anonymous", "EMPLOYEE@DARYUS.COM", "EMPLOYEE@DARYUS.COM", "employee@daryus.com" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cb6397fe-acf8-49dd-b791-01bf0b069aee",
                columns: new[] { "DateOfBirth", "Email", "FirstName", "LastName", "NormalizedEmail", "NormalizedUserName", "UserName" },
                values: new object[] { new DateOnly(1970, 11, 7), "admin@daryus.com", "Administrator", "Anonymous", "ADMIN@DARYUS.COM", "ADMIN@DARYUS.COM", "admin@daryus.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8a862852-9e68-4bcc-b624-220e9b060cf9",
                columns: new[] { "DateOfBirth", "Email", "FirstName", "LastName", "NormalizedEmail", "NormalizedUserName", "UserName" },
                values: new object[] { new DateOnly(1991, 7, 1), "admin_bu1@localhost.com", "Admin_Bu1", "Default", "ADMIN_BU1@LOCALHOST.COM", "ADMIN_BU1@LOCALHOST.COM", "admin_bu1@localhost.com" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a23d75b8-c842-4164-9cb1-f9e7c2366c3b",
                columns: new[] { "DateOfBirth", "Email", "FirstName", "LastName", "NormalizedEmail", "NormalizedUserName", "UserName" },
                values: new object[] { new DateOnly(1992, 7, 1), "testuser@leavemanagement.com", "test", "user", "TESTUSER@LEAVEMANAGEMENT.COM", "TESTUSER@LEAVEMANAGEMENT.COM", "testuser@leavemanagement.com" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cb6397fe-acf8-49dd-b791-01bf0b069aee",
                columns: new[] { "DateOfBirth", "Email", "FirstName", "LastName", "NormalizedEmail", "NormalizedUserName", "UserName" },
                values: new object[] { new DateOnly(1990, 7, 1), "admin@localhost.com", "Admin", "Default", "ADMIN@LOCALHOST.COM", "ADMIN@LOCALHOST.COM", "admin@localhost.com" });
        }
    }
}
