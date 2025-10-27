using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateDeleteBehaviorInProductColorJoinAndProductSizeJoin2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_productColorJoinTable_Products_ProductId",
                table: "productColorJoinTable");

            migrationBuilder.AddForeignKey(
                name: "FK_productColorJoinTable_Products_ProductId",
                table: "productColorJoinTable",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_productColorJoinTable_Products_ProductId",
                table: "productColorJoinTable");

            migrationBuilder.AddForeignKey(
                name: "FK_productColorJoinTable_Products_ProductId",
                table: "productColorJoinTable",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
