using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateDeleteBehaviorInProductColorJoinAndProductSizeJoin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_productColorJoinTable_ProductColors_ProductColorId",
                table: "productColorJoinTable");

            migrationBuilder.DropForeignKey(
                name: "FK_productSizeJoinTable_ProductSizes_ProductSizeId",
                table: "productSizeJoinTable");

            migrationBuilder.DropForeignKey(
                name: "FK_productSizeJoinTable_Products_ProductId",
                table: "productSizeJoinTable");

            migrationBuilder.AddForeignKey(
                name: "FK_productColorJoinTable_ProductColors_ProductColorId",
                table: "productColorJoinTable",
                column: "ProductColorId",
                principalTable: "ProductColors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_productSizeJoinTable_ProductSizes_ProductSizeId",
                table: "productSizeJoinTable",
                column: "ProductSizeId",
                principalTable: "ProductSizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_productSizeJoinTable_Products_ProductId",
                table: "productSizeJoinTable",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_productColorJoinTable_ProductColors_ProductColorId",
                table: "productColorJoinTable");

            migrationBuilder.DropForeignKey(
                name: "FK_productSizeJoinTable_ProductSizes_ProductSizeId",
                table: "productSizeJoinTable");

            migrationBuilder.DropForeignKey(
                name: "FK_productSizeJoinTable_Products_ProductId",
                table: "productSizeJoinTable");

            migrationBuilder.AddForeignKey(
                name: "FK_productColorJoinTable_ProductColors_ProductColorId",
                table: "productColorJoinTable",
                column: "ProductColorId",
                principalTable: "ProductColors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_productSizeJoinTable_ProductSizes_ProductSizeId",
                table: "productSizeJoinTable",
                column: "ProductSizeId",
                principalTable: "ProductSizes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_productSizeJoinTable_Products_ProductId",
                table: "productSizeJoinTable",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
