using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace crewlinkship.Migrations
{
    public partial class _26062025 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblImportBudgetCode",
                columns: table => new
                {
                    BudgetCodeId = table.Column<int>(type: "int", nullable: true),
                    BudgetCodeDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BudgetCodes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    RecDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TblImportBudgetSubCode",
                columns: table => new
                {
                    SubCodeId = table.Column<int>(type: "int", nullable: true),
                    BudgetCodeId = table.Column<int>(type: "int", nullable: false),
                    SubCode = table.Column<int>(type: "int", nullable: false),
                    SubBudget = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    RecDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TblImportCBA",
                columns: table => new
                {
                    CBAId = table.Column<int>(type: "int", nullable: true),
                    CBAName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CBADescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Attachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPF = table.Column<bool>(type: "bit", nullable: true),
                    IsGratuity = table.Column<bool>(type: "bit", nullable: true),
                    IsAVC = table.Column<bool>(type: "bit", nullable: true),
                    IsNUSI = table.Column<bool>(type: "bit", nullable: true),
                    CBAUnionId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    RecDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TblImportcity",
                columns: table => new
                {
                    CityId = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    StateId = table.Column<int>(type: "int", nullable: true),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DomAirport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntAirport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    RecDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TblImportCourseRegister",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: true),
                    CourseName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Method = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RankId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Group = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiryApplicable = table.Column<bool>(type: "bit", nullable: true),
                    RenewalRequired = table.Column<bool>(type: "bit", nullable: true),
                    AuthenticationRequired = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    RecDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TblImportOverTime",
                columns: table => new
                {
                    OTId = table.Column<int>(type: "int", nullable: true),
                    RankId = table.Column<int>(type: "int", nullable: false),
                    OTRate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CBAId = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    RecDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TblImportSeaport",
                columns: table => new
                {
                    SeaportId = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    SeaportName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TblImportState",
                columns: table => new
                {
                    StateId = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    StateName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    RecDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TblImportWageComponent",
                columns: table => new
                {
                    WageId = table.Column<int>(type: "int", nullable: true),
                    BudgetCodeId = table.Column<int>(type: "int", nullable: true),
                    SubCodeId = table.Column<int>(type: "int", nullable: true),
                    CalculationBasis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayableBasis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IncludedOnboard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Earning = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCBA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsShowAll = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    RecDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ColumnConfigId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TblImportWageStructure",
                columns: table => new
                {
                    WageStructureId = table.Column<int>(type: "int", nullable: true),
                    RankId = table.Column<int>(type: "int", nullable: false),
                    WageId = table.Column<int>(type: "int", nullable: true),
                    WageAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CBAId = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    RecDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblImportBudgetCode");

            migrationBuilder.DropTable(
                name: "TblImportBudgetSubCode");

            migrationBuilder.DropTable(
                name: "TblImportCBA");

            migrationBuilder.DropTable(
                name: "TblImportcity");

            migrationBuilder.DropTable(
                name: "TblImportCourseRegister");

            migrationBuilder.DropTable(
                name: "TblImportOverTime");

            migrationBuilder.DropTable(
                name: "TblImportSeaport");

            migrationBuilder.DropTable(
                name: "TblImportState");

            migrationBuilder.DropTable(
                name: "TblImportWageComponent");

            migrationBuilder.DropTable(
                name: "TblImportWageStructure");
        }
    }
}
