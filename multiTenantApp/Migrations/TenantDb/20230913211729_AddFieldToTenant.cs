using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace multiTenantApp.Migrations.TenantDb
{
    /// <inheritdoc />
    public partial class AddFieldToTenant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubscriptionLevel",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscriptionLevel",
                table: "Tenants");
        }
    }
}
