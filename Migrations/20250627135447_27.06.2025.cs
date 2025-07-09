using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace crewlinkship.Migrations
{
    public partial class _27062025 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblImportCBAUnion",
                columns: table => new
                {
                    UnionId = table.Column<int>(type: "int", nullable: true),
                    UnionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RankGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    RecDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblImportCBAUnion");
        }
    }
}
