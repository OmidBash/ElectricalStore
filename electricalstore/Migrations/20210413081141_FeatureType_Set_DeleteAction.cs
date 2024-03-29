﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace electricalstore.Migrations
{
    public partial class FeatureType_Set_DeleteAction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Image",
                schema: "Store",
                table: "ProductImages");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Image",
                schema: "Store",
                table: "ProductImages",
                column: "ProductId",
                principalSchema: "Store",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Image",
                schema: "Store",
                table: "ProductImages");

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
    }
}
