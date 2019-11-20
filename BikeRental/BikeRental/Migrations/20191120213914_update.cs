using Microsoft.EntityFrameworkCore.Migrations;

namespace BikeRental.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Rentals",
                table: "Rentals");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Rentals",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rentals",
                table: "Rentals",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_CustomerID",
                table: "Rentals",
                column: "CustomerID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Rentals",
                table: "Rentals");

            migrationBuilder.DropIndex(
                name: "IX_Rentals_CustomerID",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Rentals");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rentals",
                table: "Rentals",
                columns: new[] { "CustomerID", "BikeID" });
        }
    }
}
