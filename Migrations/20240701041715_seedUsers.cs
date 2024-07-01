using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace dotnet_api.Migrations
{
    /// <inheritdoc />
    public partial class seedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "56808ba4-6b4b-4955-8a0e-65a14605349e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c70a06fa-eb42-4ece-8653-2f4b18640095");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b15ddf11-7a08-45c4-b533-dab487138015", null, "Admin", "ADMIN" },
                    { "fad525d5-2c80-4a7a-878e-5132d5125292", null, "user", "CUSTOMER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "1", 0, "9436a425-8245-41f3-94a2-bab4564f036b", "admin@gmail.com", true, "Admin", "One", false, null, "admin@gmail.com", "ADMIN", "AQAAAAIAAYagAAAAEK5SEAuCqnCQ109QR16bkOVF/LqOTK4CU/JCrfzJHPzbq3GuQCon8sWJH8y//1UdtQ==", null, false, "", false, "admin" },
                    { "2", 0, "deca7c62-24f0-4886-a856-687ec6a08487", "cust1@gmail.com", true, "Customer", "One", false, null, "cust1@gmail.com", "cust 1", "AQAAAAIAAYagAAAAEBAUYOTnyjrZg4NDKFFM4EWyApYH1sm6BDyVRL7OBIKxuPRpp2y2qLVnqWJKIBsSAQ==", null, false, "", false, "cust1" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "b15ddf11-7a08-45c4-b533-dab487138015", "1" },
                    { "fad525d5-2c80-4a7a-878e-5132d5125292", "2" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "b15ddf11-7a08-45c4-b533-dab487138015", "1" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "fad525d5-2c80-4a7a-878e-5132d5125292", "2" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b15ddf11-7a08-45c4-b533-dab487138015");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fad525d5-2c80-4a7a-878e-5132d5125292");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "56808ba4-6b4b-4955-8a0e-65a14605349e", null, "user", "CUSTOMER" },
                    { "c70a06fa-eb42-4ece-8653-2f4b18640095", null, "Admin", "ADMIN" }
                });
        }
    }
}
