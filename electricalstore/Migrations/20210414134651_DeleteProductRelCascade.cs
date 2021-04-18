using Microsoft.EntityFrameworkCore.Migrations;

namespace electricalstore.Migrations
{
    public partial class DeleteProductRelCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Category",
                schema: "Store",
                table: "CategoryProductJunction");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Feature",
                schema: "Store",
                table: "ProductFeatureJunction");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Category",
                schema: "Store",
                table: "CategoryProductJunction",
                column: "ProductId",
                principalSchema: "Store",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Feature",
                schema: "Store",
                table: "ProductFeatureJunction",
                column: "ProductId",
                principalSchema: "Store",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Category",
                schema: "Store",
                table: "CategoryProductJunction");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Feature",
                schema: "Store",
                table: "ProductFeatureJunction");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Category",
                schema: "Store",
                table: "CategoryProductJunction",
                column: "ProductId",
                principalSchema: "Store",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Feature",
                schema: "Store",
                table: "ProductFeatureJunction",
                column: "ProductId",
                principalSchema: "Store",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
