using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace multiTenantApp.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class AddFieldToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Supplier",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Supplier",
                table: "Products");
        }
    }
}
