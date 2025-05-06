using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedApplicationUserConfigData2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cb6397fe-acf8-49dd-b791-01bf0b069aee",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJayflAw5VX1+ms/Pdn0L7/PDMCtliDSisLaP6QeKibeCZXGFGZEp14Oq8CKhOztHw==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cb6397fe-acf8-49dd-b791-01bf0b069aee",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEN5WCPZ+e5Tcc6puplTNrflD+R6WpF82fsT2aCWMlDDmwAlhys5FMsfFVgax4+GI7Q==");
        }
    }
}
