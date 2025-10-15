using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditInBasketTables3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItems_Baskets_BasketId1",
                table: "BasketItems");

            migrationBuilder.DropIndex(
                name: "IX_BasketItems_BasketId1",
                table: "BasketItems");

            migrationBuilder.DropColumn(
                name: "BasketId1",
                table: "BasketItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BasketId1",
                table: "BasketItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BasketItems_BasketId1",
                table: "BasketItems",
                column: "BasketId1");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItems_Baskets_BasketId1",
                table: "BasketItems",
                column: "BasketId1",
                principalTable: "Baskets",
                principalColumn: "Id");
        }
    }
}
