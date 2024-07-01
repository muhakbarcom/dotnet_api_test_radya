using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace dotnet_api.Migrations
{
    /// <inheritdoc />
    public partial class bookSeeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "f5a79dcf-b3b2-42ef-ad0e-c68d3d586175", "1" });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1aaa0091-cef0-489c-bd9e-2b9198278a98", "2" });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "1aaa0091-cef0-489c-bd9e-2b9198278a98");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "f5a79dcf-b3b2-42ef-ad0e-c68d3d586175");

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "Genre", "Price", "Quantity", "Title" },
                values: new object[,]
                {
                    { 1, "Author 1", "Fiction", 100000m, 10, "Book 1" },
                    { 2, "Author 2", "Fiction", 200000m, 20, "Book 2" },
                    { 3, "Author 3", "Fiction", 300000m, 30, "Book 3" },
                    { 4, "Author 4", "Fiction", 400000m, 40, "Book 4" },
                    { 5, "Author 5", "Action", 500000m, 50, "Book 5" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "268af40e-07f7-416f-805c-3ab1df652f24", null, "user", "CUSTOMER" },
                    { "87e076f7-8ddf-465b-9901-8c159eace3e0", null, "Admin", "ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "88409dff-fdf8-44a1-95a8-e28207e73de7", "AQAAAAIAAYagAAAAEL6WSaWEkxTNryV0PiplEul3e97imcc5LQv8c4VuWiwFrqHL+LCTNTuXikq/8uq61A==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "fa5a9f70-550e-4a46-a0ce-7b0c0ce8b054", "AQAAAAIAAYagAAAAEAGds7eTM59rZYUfgs+TthW0wCrGQ9uZy2WdYnuDOfzP4jouZHHQgax3kAJrjq/GLA==" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "87e076f7-8ddf-465b-9901-8c159eace3e0", "1" },
                    { "268af40e-07f7-416f-805c-3ab1df652f24", "2" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "87e076f7-8ddf-465b-9901-8c159eace3e0", "1" });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "268af40e-07f7-416f-805c-3ab1df652f24", "2" });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "268af40e-07f7-416f-805c-3ab1df652f24");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "87e076f7-8ddf-465b-9901-8c159eace3e0");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1aaa0091-cef0-489c-bd9e-2b9198278a98", null, "user", "CUSTOMER" },
                    { "f5a79dcf-b3b2-42ef-ad0e-c68d3d586175", null, "Admin", "ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "7bd62b53-9674-4988-9b83-e68e11b90341", "AQAAAAIAAYagAAAAEACmErxfqjxG4KGB/UWR/aI6Aljar/IdKrZhhMBwCbouKh17lhK4309Yta1RJoLfng==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "faf5f806-d55e-4457-bcc7-dbdd4d94d920", "AQAAAAIAAYagAAAAEPYRWn5M6skqDTqj0nHI1+RN7nyZ1p+zM2mopjbLwrNMvD8hYqJv744atU9f+gEnCw==" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "f5a79dcf-b3b2-42ef-ad0e-c68d3d586175", "1" },
                    { "1aaa0091-cef0-489c-bd9e-2b9198278a98", "2" }
                });
        }
    }
}
