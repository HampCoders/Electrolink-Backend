using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Hampcoders.Electrolink.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "component_types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_component_types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "properties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    address_street = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    address_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    address_city = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    address_postal_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    address_country = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    region_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    region_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    district_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    district_ubigeo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_properties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "technician_inventories",
                columns: table => new
                {
                    TechnicianId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_technician_inventories", x => x.TechnicianId);
                });

            migrationBuilder.CreateTable(
                name: "components",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    component_type_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_components", x => x.Id);
                    table.ForeignKey(
                        name: "FK_components_component_types_component_type_id",
                        column: x => x.component_type_id,
                        principalTable: "component_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "property_photos",
                columns: table => new
                {
                    PhotoURL = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    property_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_property_photos", x => new { x.property_id, x.PhotoURL });
                    table.ForeignKey(
                        name: "FK_property_photos_properties_property_id",
                        column: x => x.property_id,
                        principalTable: "properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "component_stocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TechnicianInventoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    ComponentId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuantityAvailable = table.Column<int>(type: "integer", nullable: false),
                    AlertThreshold = table.Column<int>(type: "integer", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_component_stocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_component_stocks_components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_component_stocks_technician_inventories_TechnicianInventory~",
                        column: x => x.TechnicianInventoryId,
                        principalTable: "technician_inventories",
                        principalColumn: "TechnicianId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_component_stocks_ComponentId",
                table: "component_stocks",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_component_stocks_TechnicianInventoryId_ComponentId",
                table: "component_stocks",
                columns: new[] { "TechnicianInventoryId", "ComponentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_components_component_type_id",
                table: "components",
                column: "component_type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "component_stocks");

            migrationBuilder.DropTable(
                name: "property_photos");

            migrationBuilder.DropTable(
                name: "components");

            migrationBuilder.DropTable(
                name: "technician_inventories");

            migrationBuilder.DropTable(
                name: "properties");

            migrationBuilder.DropTable(
                name: "component_types");
        }
    }
}
