using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace crewlinkship.Migrations
{
    public partial class _13062025 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OCIMFVMs",
                columns: table => new
                {
                    OCIMFVMId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VesselName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FamilyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiddleInitial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Identification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CertofComp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuingCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdminAccept = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TankerCert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecializedTankerTraining = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RadioQual = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnglishProf = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignOnDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OperatorExp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RankExperience = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TankerExp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AllTankerExp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalExperience = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OCIMFVMs", x => x.OCIMFVMId);
                });

            migrationBuilder.CreateTable(
                name: "portageBillBows",
                columns: table => new
                {
                    pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PortageBillId = table.Column<int>(type: "int", nullable: true),
                    CrewId = table.Column<int>(type: "int", nullable: true),
                    ContractId = table.Column<int>(type: "int", nullable: true),
                    numberofday = table.Column<int>(type: "int", nullable: true),
                    CrewName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cdcno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rankid = table.Column<int>(type: "int", nullable: true),
                    AppliedCBA = table.Column<int>(type: "int", nullable: true),
                    Paydate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SignOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    From = table.Column<DateTime>(type: "datetime2", nullable: true),
                    To = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalDays = table.Column<int>(type: "int", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BasicWages = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FixedGtOT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SecurityAllow = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LeavePay = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UniformAllow = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TempFuelAllow = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PensionFund = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SpecialAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CompanyAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IncentiveAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Seniority = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TankerAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Housing = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Transport = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Utility = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Bonus = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SeafarersPF = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    leavepayaddition = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OtherAllow = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalWages = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CrewListId = table.Column<int>(type: "int", nullable: true),
                    OTHours = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OtherEarnings = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TransitDays = table.Column<int>(type: "int", nullable: true),
                    TransitWages = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CashAdvance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BondedStores = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OtherDeductions = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Allotments = table.Column<int>(type: "int", nullable: true),
                    PFAmount = table.Column<int>(type: "int", nullable: true),
                    UDAmount = table.Column<int>(type: "int", nullable: true),
                    WFAmount = table.Column<int>(type: "int", nullable: true),
                    Deduction = table.Column<int>(type: "int", nullable: true),
                    Reimbursement = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    otrate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    extraot = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LeaveWagesBF = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalDeductions = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalPayable = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PrevMonthBal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FinalBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SignOffDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vessel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vesselid = table.Column<int>(type: "int", nullable: true),
                    bankid = table.Column<int>(type: "int", nullable: true),
                    Tax = table.Column<int>(type: "int", nullable: true),
                    UDdeduction = table.Column<int>(type: "int", nullable: true),
                    WFdeduction = table.Column<int>(type: "int", nullable: true),
                    WHTdeduction = table.Column<int>(type: "int", nullable: true),
                    authorityDeduction = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AllTotalDays = table.Column<int>(type: "int", nullable: true),
                    createddate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    createdby = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modifyby = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsLeaveWagesCF = table.Column<int>(type: "int", nullable: false),
                    Attachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gratuity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IndPFAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AVC = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsAddPrevBal = table.Column<bool>(type: "bit", nullable: false),
                    MidMonthAllotment = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_portageBillBows", x => x.pid);
                });

            migrationBuilder.CreateTable(
                name: "PortageBillPDFSignoffVM",
                columns: table => new
                {
                    pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PortageBillId = table.Column<int>(type: "int", nullable: true),
                    CrewId = table.Column<int>(type: "int", nullable: true),
                    ContractId = table.Column<int>(type: "int", nullable: true),
                    numberofday = table.Column<int>(type: "int", nullable: true),
                    CrewName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cdcno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rankid = table.Column<int>(type: "int", nullable: true),
                    AppliedCBA = table.Column<int>(type: "int", nullable: true),
                    Paydate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SignOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    From = table.Column<DateTime>(type: "datetime2", nullable: true),
                    To = table.Column<DateTime>(type: "datetime2", nullable: true),
                    duration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalDays = table.Column<int>(type: "int", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WSBasicWages = table.Column<int>(type: "int", nullable: true),
                    WSFixedGtOT = table.Column<int>(type: "int", nullable: true),
                    WSLeavePay = table.Column<int>(type: "int", nullable: true),
                    WSUniformAllow = table.Column<int>(type: "int", nullable: true),
                    WSPensionFund = table.Column<int>(type: "int", nullable: true),
                    WSSpecialAllowance = table.Column<int>(type: "int", nullable: true),
                    WSCompanyAllowance = table.Column<int>(type: "int", nullable: true),
                    WSIncentiveAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WSSeniority = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WSTankerAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WSHousing = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WSTransport = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WSUtility = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WSBonus = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WSSeafarersPF = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WSleavepayaddition = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WSSecurityAllow = table.Column<int>(type: "int", nullable: true),
                    WSTempFuelAllow = table.Column<int>(type: "int", nullable: true),
                    WSOtherAllow = table.Column<int>(type: "int", nullable: true),
                    WSTotalWages = table.Column<int>(type: "int", nullable: true),
                    otrate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OTHours = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BasicWages = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FixedGtOT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LeavePay = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UniformAllow = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PensionFund = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SpecialAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CompanyAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IncentiveAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Seniority = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TankerAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Housing = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Transport = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Utility = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Bonus = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SeafarersPF = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    leavepayaddition = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SecurityAllow = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TempFuelAllow = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OtherAllow = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    extraot = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OtherEarnings = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TransitDays = table.Column<int>(type: "int", nullable: true),
                    TransitWages = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalWages = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CrewListId = table.Column<int>(type: "int", nullable: true),
                    PrevMonthBal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Reimbursement = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalPayable = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LeaveWagesDed = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CashAdvance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BondedStores = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OtherDeductions = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PFAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PFAmount10 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UDAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WFAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Allotments = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Deduction = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    authorityDeduction = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalDeductions = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LeaveWagesBF = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LeaveWagesCF = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FinalBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SignOffDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vessel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EPF = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsLeaveWagesCF = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortageBillPDFSignoffVM", x => x.pid);
                });

            migrationBuilder.CreateTable(
                name: "PortageBillPDFVM",
                columns: table => new
                {
                    pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PortageBillId = table.Column<int>(type: "int", nullable: true),
                    CrewId = table.Column<int>(type: "int", nullable: true),
                    ContractId = table.Column<int>(type: "int", nullable: true),
                    numberofday = table.Column<int>(type: "int", nullable: true),
                    CrewName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cdcno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rankid = table.Column<int>(type: "int", nullable: true),
                    AppliedCBA = table.Column<int>(type: "int", nullable: true),
                    duration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Paydate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SignOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    From = table.Column<DateTime>(type: "datetime2", nullable: true),
                    To = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalDays = table.Column<int>(type: "int", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WSBasicWages = table.Column<int>(type: "int", nullable: true),
                    WSFixedGtOT = table.Column<int>(type: "int", nullable: true),
                    WSLeavePay = table.Column<int>(type: "int", nullable: true),
                    WSUniformAllow = table.Column<int>(type: "int", nullable: true),
                    WSPensionFund = table.Column<int>(type: "int", nullable: true),
                    WSSpecialAllowance = table.Column<int>(type: "int", nullable: true),
                    WSCompanyAllowance = table.Column<int>(type: "int", nullable: true),
                    WSIncentiveAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WSSeniority = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WSTankerAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WSHousing = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WSTransport = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WSUtility = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WSBonus = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WSSeafarersPF = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WSleavepayaddition = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WSSecurityAllow = table.Column<int>(type: "int", nullable: true),
                    WSTempFuelAllow = table.Column<int>(type: "int", nullable: true),
                    WSOtherAllow = table.Column<int>(type: "int", nullable: true),
                    WSTotalWages = table.Column<int>(type: "int", nullable: true),
                    otrate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OTHours = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BasicWages = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FixedGtOT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LeavePay = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UniformAllow = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PensionFund = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SpecialAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CompanyAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IncentiveAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Seniority = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TankerAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Housing = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Transport = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Utility = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Bonus = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SeafarersPF = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    leavepayaddition = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SecurityAllow = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TempFuelAllow = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OtherAllow = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    extraot = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OtherEarnings = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TransitDays = table.Column<int>(type: "int", nullable: true),
                    TransitWages = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalWages = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CrewListId = table.Column<int>(type: "int", nullable: true),
                    PrevMonthBal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Reimbursement = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalPayable = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LeaveWagesDed = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CashAdvance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BondedStores = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OtherDeductions = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PFAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PFAmount10 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UDAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WFAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Allotments = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Deduction = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    authorityDeduction = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalDeductions = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LeaveWagesBF = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LeaveWagesCF = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FinalBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EPF = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SignOffDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vessel = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortageBillPDFVM", x => x.pid);
                });

            migrationBuilder.CreateTable(
                name: "PortageBillSignoffVM",
                columns: table => new
                {
                    pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PortageBillId = table.Column<int>(type: "int", nullable: true),
                    CrewId = table.Column<int>(type: "int", nullable: true),
                    ContractId = table.Column<int>(type: "int", nullable: true),
                    numberofday = table.Column<int>(type: "int", nullable: true),
                    CrewName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cdcno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rankid = table.Column<int>(type: "int", nullable: true),
                    AppliedCBA = table.Column<int>(type: "int", nullable: true),
                    Paydate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SignOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    From = table.Column<DateTime>(type: "datetime2", nullable: true),
                    To = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalDays = table.Column<int>(type: "int", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BasicWages = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FixedGtOT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SecurityAllow = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LeavePay = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UniformAllow = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TempFuelAllow = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PensionFund = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SpecialAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CompanyAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IncentiveAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Seniority = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TankerAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Housing = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Transport = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Utility = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Bonus = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SeafarersPF = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    leavepayaddition = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    contractbonus = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    contractreimbrusment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    contractdedu = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OtherAllow = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalWages = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CrewListId = table.Column<int>(type: "int", nullable: true),
                    OTHours = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OtherEarnings = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TransitDays = table.Column<int>(type: "int", nullable: true),
                    TransitWages = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CashAdvance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BondedStores = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OtherDeductions = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Allotments = table.Column<int>(type: "int", nullable: true),
                    PFAmount = table.Column<int>(type: "int", nullable: true),
                    UDAmount = table.Column<int>(type: "int", nullable: true),
                    WFAmount = table.Column<int>(type: "int", nullable: true),
                    Deduction = table.Column<int>(type: "int", nullable: true),
                    authorityDeduction = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Reimbursement = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    otrate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    extraot = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LeaveWagesBF = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalDeductions = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalPayable = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PrevMonthBal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FinalBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SignOffDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vessel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vesselid = table.Column<int>(type: "int", nullable: true),
                    bankid = table.Column<int>(type: "int", nullable: true),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UDdeduction = table.Column<int>(type: "int", nullable: true),
                    WFdeduction = table.Column<int>(type: "int", nullable: true),
                    WHTdeduction = table.Column<int>(type: "int", nullable: true),
                    createddate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    createdby = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modifyby = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AllTotalDays = table.Column<int>(type: "int", nullable: true),
                    signoffreason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ispromoted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Islocked = table.Column<int>(type: "int", nullable: false),
                    IsLeaveWagesCF = table.Column<int>(type: "int", nullable: false),
                    Attachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gratuity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IndPFAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AVC = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsAddPrevBal = table.Column<bool>(type: "bit", nullable: false),
                    MidMonthAllotment = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortageBillSignoffVM", x => x.pid);
                });

            migrationBuilder.CreateTable(
                name: "PortageBillVM",
                columns: table => new
                {
                    pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PortageBillId = table.Column<int>(type: "int", nullable: true),
                    CrewId = table.Column<int>(type: "int", nullable: true),
                    ContractId = table.Column<int>(type: "int", nullable: true),
                    numberofday = table.Column<int>(type: "int", nullable: true),
                    CrewName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cdcno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rankid = table.Column<int>(type: "int", nullable: true),
                    AppliedCBA = table.Column<int>(type: "int", nullable: true),
                    Paydate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SignOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    From = table.Column<DateTime>(type: "datetime2", nullable: true),
                    To = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalDays = table.Column<int>(type: "int", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BasicWages = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FixedGtOT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SecurityAllow = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LeavePay = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UniformAllow = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TempFuelAllow = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PensionFund = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SpecialAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CompanyAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IncentiveAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Seniority = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TankerAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Housing = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Transport = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Utility = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Bonus = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SeafarersPF = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    leavepayaddition = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    contractbonus = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    contractreimbrusment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    contractdedu = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OtherAllow = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalWages = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CrewListId = table.Column<int>(type: "int", nullable: true),
                    OTHours = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OtherEarnings = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TransitDays = table.Column<int>(type: "int", nullable: true),
                    TransitWages = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CashAdvance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BondedStores = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OtherDeductions = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Allotments = table.Column<int>(type: "int", nullable: true),
                    PFAmount = table.Column<int>(type: "int", nullable: true),
                    UDAmount = table.Column<int>(type: "int", nullable: true),
                    WFAmount = table.Column<int>(type: "int", nullable: true),
                    Deduction = table.Column<int>(type: "int", nullable: true),
                    authorityDeduction = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Reimbursement = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    otrate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    extraot = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LeaveWagesBF = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalDeductions = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalPayable = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PrevMonthBal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FinalBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SignOffDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vessel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vesselid = table.Column<int>(type: "int", nullable: true),
                    bankid = table.Column<int>(type: "int", nullable: true),
                    Tax = table.Column<int>(type: "int", nullable: true),
                    UDdeduction = table.Column<int>(type: "int", nullable: true),
                    WFdeduction = table.Column<int>(type: "int", nullable: true),
                    WHTdeduction = table.Column<int>(type: "int", nullable: true),
                    AllTotalDays = table.Column<int>(type: "int", nullable: true),
                    createddate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    modifieddate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    createdby = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modifyby = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ispromoted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    signoffreason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Islocked = table.Column<int>(type: "int", nullable: false),
                    Attachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gratuity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IndPFAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AVC = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    bowRequest = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAddPrevBal = table.Column<bool>(type: "bit", nullable: false),
                    MidMonthAllotment = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortageBillVM", x => x.pid);
                });

          

           

         

           

         

           

            migrationBuilder.CreateTable(
                name: "TblImportBudgetCode",
                columns: table => new
                {
                    BudgetCodeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BudgetCodeDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BudgetCodes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    RecDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblImportBudgetCode", x => x.BudgetCodeId);
                });

            migrationBuilder.CreateTable(
                name: "TblImportBudgetSubCode",
                columns: table => new
                {
                    SubCodeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    table.PrimaryKey("PK_TblImportBudgetSubCode", x => x.SubCodeId);
                });

            migrationBuilder.CreateTable(
                name: "TblImportCBA",
                columns: table => new
                {
                    CBAId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    table.PrimaryKey("PK_TblImportCBA", x => x.CBAId);
                });

         

            migrationBuilder.CreateTable(
                name: "TblImportCourseRegister",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    table.PrimaryKey("PK_TblImportCourseRegister", x => x.CourseId);
                });  

            migrationBuilder.CreateTable(
                name: "TblImportOverTime",
                columns: table => new
                {
                    OTId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    table.PrimaryKey("PK_TblImportOverTime", x => x.OTId);
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
                name: "TblImportCdc");

            migrationBuilder.DropTable(
                name: "TblImportContract");

            migrationBuilder.DropTable(
                name: "TblImportContractReimDedu");

            migrationBuilder.DropTable(
                name: "TblImportCourseRegister");

            migrationBuilder.DropTable(
                name: "TblImportCrewAddress");

            migrationBuilder.DropTable(
                name: "TblImportCrewBankDetail");

            migrationBuilder.DropTable(
                name: "tblImportCrewCorAddress");

            migrationBuilder.DropTable(
                name: "TblImportCrewCourse");

            migrationBuilder.DropTable(
                name: "TblImportCrewDetail");

            migrationBuilder.DropTable(
                name: "TblImportCrewLicense");

            migrationBuilder.DropTable(
                name: "TblImportCrewList");

            migrationBuilder.DropTable(
                name: "TblImportCrewOtherDocument");

            migrationBuilder.DropTable(
                name: "TblImportMidMonthAllotment");

            migrationBuilder.DropTable(
                name: "TblImportNigerianDeduction");

            migrationBuilder.DropTable(
                name: "TblImportOverTime");

            migrationBuilder.DropTable(
                name: "tblImportPassport");

            migrationBuilder.DropTable(
                name: "tblImportPBBankAllotment");

            migrationBuilder.DropTable(
                name: "TblImportPortageBill");

            migrationBuilder.DropTable(
                name: "tblimportPortageEarningDeduction");

            migrationBuilder.DropTable(
                name: "TblImportTransferCrew");

            migrationBuilder.DropTable(
                name: "tblImportVessel");

            migrationBuilder.DropTable(
                name: "tblImportVesselCBA");

            migrationBuilder.DropTable(
                name: "TblImportVisa");

            migrationBuilder.DropTable(
                name: "TblImportWageComponent");

            migrationBuilder.DropTable(
                name: "TblImportWageStructure");

            migrationBuilder.DropTable(
                name: "TblImportYellowfever");

            migrationBuilder.DropTable(
                name: "tblIssuingAuthority");

            migrationBuilder.DropTable(
                name: "tblLockPortageBill");

            migrationBuilder.DropTable(
                name: "tblMidMonthAllotment");

            migrationBuilder.DropTable(
                name: "tblNigerianDeduction");

            migrationBuilder.DropTable(
                name: "tblOverTime");

            migrationBuilder.DropTable(
                name: "tblPassport");

            migrationBuilder.DropTable(
                name: "tblPBBankAllotment");

            migrationBuilder.DropTable(
                name: "tblPFRate");

            migrationBuilder.DropTable(
                name: "tblPortageBill");

            migrationBuilder.DropTable(
                name: "tblPortageEarningDeduction");

            migrationBuilder.DropTable(
                name: "tblReimbursementOrDeduction");

            migrationBuilder.DropTable(
                name: "tblTransferCrew");

            migrationBuilder.DropTable(
                name: "tblVesselCBA");

            migrationBuilder.DropTable(
                name: "tblVesselChange");

            migrationBuilder.DropTable(
                name: "tblVisa");

            migrationBuilder.DropTable(
                name: "tblWageStructure");

            migrationBuilder.DropTable(
                name: "tblYellowfever");

            migrationBuilder.DropTable(
                name: "Userlogin");

            migrationBuilder.DropTable(
                name: "tblSignOnReason");

            migrationBuilder.DropTable(
                name: "tblSignOffReason");

            migrationBuilder.DropTable(
                name: "tblCourseRegister");

            migrationBuilder.DropTable(
                name: "tblInstitutes");

            migrationBuilder.DropTable(
                name: "tblLicenceRegister");

            migrationBuilder.DropTable(
                name: "tblAuthority");

            migrationBuilder.DropTable(
                name: "tblOtherDocuments");

            migrationBuilder.DropTable(
                name: "tblContract");

            migrationBuilder.DropTable(
                name: "tblCrewList");

            migrationBuilder.DropTable(
                name: "tblCBA");

            migrationBuilder.DropTable(
                name: "tblWageComponent");

            migrationBuilder.DropTable(
                name: "tblCrewDetails");

            migrationBuilder.DropTable(
                name: "tblCbaUnion");

            migrationBuilder.DropTable(
                name: "tblBudgetSubCode");

            migrationBuilder.DropTable(
                name: "tblRankRegister");

            migrationBuilder.DropTable(
                name: "tblVessel");

            migrationBuilder.DropTable(
                name: "tblBudgetCode");

            migrationBuilder.DropTable(
                name: "tblBuilders");

            migrationBuilder.DropTable(
                name: "tblClassification");

            migrationBuilder.DropTable(
                name: "tblDisponentOwner");

            migrationBuilder.DropTable(
                name: "tblECDIS");

            migrationBuilder.DropTable(
                name: "tblEngineModel");

            migrationBuilder.DropTable(
                name: "tblManager");

            migrationBuilder.DropTable(
                name: "tblOwner");

            migrationBuilder.DropTable(
                name: "tblPool");

            migrationBuilder.DropTable(
                name: "tblPrincipal");

            migrationBuilder.DropTable(
                name: "tblShipType");

            migrationBuilder.DropTable(
                name: "tblVendorRegister");

            migrationBuilder.DropTable(
                name: "tblEngineSubType");

            migrationBuilder.DropTable(
                name: "tblCity");

            migrationBuilder.DropTable(
                name: "tblSeaport");

            migrationBuilder.DropTable(
                name: "tblEnginetype");

            migrationBuilder.DropTable(
                name: "tblState");

            migrationBuilder.DropTable(
                name: "tblCountry");
        }
    }
}
