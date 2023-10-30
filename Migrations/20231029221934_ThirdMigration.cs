using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace productscategories.Migrations
{
    public partial class ThirdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asssociations_Categories_CategorieCategoryId",
                table: "Asssociations");

            migrationBuilder.DropIndex(
                name: "IX_Asssociations_CategorieCategoryId",
                table: "Asssociations");

            migrationBuilder.DropColumn(
                name: "CategorieCategoryId",
                table: "Asssociations");

            migrationBuilder.CreateIndex(
                name: "IX_Asssociations_CategoryId",
                table: "Asssociations",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Asssociations_Categories_CategoryId",
                table: "Asssociations",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asssociations_Categories_CategoryId",
                table: "Asssociations");

            migrationBuilder.DropIndex(
                name: "IX_Asssociations_CategoryId",
                table: "Asssociations");

            migrationBuilder.AddColumn<int>(
                name: "CategorieCategoryId",
                table: "Asssociations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Asssociations_CategorieCategoryId",
                table: "Asssociations",
                column: "CategorieCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Asssociations_Categories_CategorieCategoryId",
                table: "Asssociations",
                column: "CategorieCategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId");
        }
    }
}
