using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditAddressAndShippingAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "ShippingAddresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FloorNumber",
                table: "ShippingAddresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Landmark",
                table: "ShippingAddresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "Address",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FloorNumber",
                table: "Address",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Landmark",
                table: "Address",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Address",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecipientName",
                table: "Address",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "District",
                table: "ShippingAddresses");

            migrationBuilder.DropColumn(
                name: "FloorNumber",
                table: "ShippingAddresses");

            migrationBuilder.DropColumn(
                name: "Landmark",
                table: "ShippingAddresses");

            migrationBuilder.DropColumn(
                name: "District",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "FloorNumber",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "Landmark",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "RecipientName",
                table: "Address");
        }
    }
}
