using Microsoft.EntityFrameworkCore.Migrations;

namespace rentasgt.Infrastructure.Persistence.Migrations
{
    public partial class UniqueIndexForAlternateKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_AspNetUsers_Cui",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Cui",
                table: "AspNetUsers",
                fixedLength: true,
                maxLength: 13,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(13) CHARACTER SET utf8mb4",
                oldFixedLength: true,
                oldMaxLength: 13);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Cui",
                table: "AspNetUsers",
                column: "Cui",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_Name",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Cui",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Cui",
                table: "AspNetUsers",
                type: "char(13) CHARACTER SET utf8mb4",
                fixedLength: true,
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldFixedLength: true,
                oldMaxLength: 13,
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AspNetUsers_Cui",
                table: "AspNetUsers",
                column: "Cui");
        }
    }
}
