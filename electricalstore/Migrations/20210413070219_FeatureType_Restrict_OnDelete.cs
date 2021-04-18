using Microsoft.EntityFrameworkCore.Migrations;

namespace electricalstore.Migrations
{
    public partial class FeatureType_Restrict_OnDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeatureType_Feature",
                schema: "Store",
                table: "Features");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Image",
                schema: "Store",
                table: "ProductImages");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureType_Feature",
                schema: "Store",
                table: "Features",
                column: "FeatureTypeId",
                principalSchema: "Store",
                principalTable: "FeatureTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Image",
                schema: "Store",
                table: "ProductImages",
                column: "ProductId",
                principalSchema: "Store",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeatureType_Feature",
                schema: "Store",
                table: "Features");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Image",
                schema: "Store",
                table: "ProductImages");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureType_Feature",
                schema: "Store",
                table: "Features",
                column: "FeatureTypeId",
                principalSchema: "Store",
                principalTable: "FeatureTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Image",
                schema: "Store",
                table: "ProductImages",
                column: "ProductId",
                principalSchema: "Store",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
