using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreApp.API.Migrations
{
    public partial class SeededDefaultUsersandRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "209fe54d-b780-41d3-a60c-d96e1c7a22f8", "43f38bf5-0efa-41a2-9ed3-63ce276cc59e", "Admin", "ADMIN" },
                    { "e4be1a4b-9126-4fed-b7f8-d3a1034b60dc", "5ab53b2f-17c9-428a-984a-d24b9c57349b", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "0e692b32-b77a-4d7d-89cb-dc1db470f448", 0, "3a5a0f3e-b16c-431c-8b10-9e751490b58b", "admin@bookstore.com", false, "System", "Admin", false, null, "ADMIN@BOOKSTORE.COM", "ADMIN", "AQAAAAEAACcQAAAAEAiw/q6JKihZ5pepQvfgsBQ0Sk0bKPLK3YbCCPLafWfO+9YcwMmQlbLFwDiDVYTATw==", null, false, "ef45a66d-46d6-45b0-804f-d2b23dc5cb9a", false, "admin@bookstore.com" },
                    { "f79beb3c-bab2-4a86-a5f2-788bb1bcf8e4", 0, "d1a98bec-e4ab-412d-839e-256240808779", "user@bookstore.com", false, "System", "USER", false, null, "USER@BOOKSTORE.COM", "USER", "AQAAAAEAACcQAAAAEIt2MnrxDqbyHo2RjAO0j2Ve7gqoBJ7b0sqwGrpym01sQzk9B4Ac4pk6wcj6xvfmdQ==", null, false, "8d341d41-94c5-44a4-9163-87cb890b0823", false, "user@bookstore.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "209fe54d-b780-41d3-a60c-d96e1c7a22f8", "0e692b32-b77a-4d7d-89cb-dc1db470f448" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "e4be1a4b-9126-4fed-b7f8-d3a1034b60dc", "f79beb3c-bab2-4a86-a5f2-788bb1bcf8e4" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "209fe54d-b780-41d3-a60c-d96e1c7a22f8", "0e692b32-b77a-4d7d-89cb-dc1db470f448" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "e4be1a4b-9126-4fed-b7f8-d3a1034b60dc", "f79beb3c-bab2-4a86-a5f2-788bb1bcf8e4" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "209fe54d-b780-41d3-a60c-d96e1c7a22f8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e4be1a4b-9126-4fed-b7f8-d3a1034b60dc");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0e692b32-b77a-4d7d-89cb-dc1db470f448");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f79beb3c-bab2-4a86-a5f2-788bb1bcf8e4");
        }
    }
}
