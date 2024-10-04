using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace crewlinkship.ViewModel
{
    public class PortageBillPDFVM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int pid { get; set; }
        public int? PortageBillId { get; set; }
        public int? CrewId { get; set; }
        public int? ContractId { get; set; }
        public int? numberofday { get; set; }
        public string CrewName { get; set; }
        public string cdcno { get; set; }
        public string RankName { get; set; }
        public string Nationality { get; set; }
        public int? Rankid { get; set; }
        public int? AppliedCBA { get; set; }
        public string duration { get; set; }
        public DateTime? Paydate { get; set; }
        public DateTime? SignOnDate { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public int? TotalDays { get; set; }
        public string Currency { get; set; }
        public int? WSBasicWages { get; set; }
        public int? WSFixedGtOT { get; set; }
        public int? WSLeavePay { get; set; }
        public int? WSUniformAllow { get; set; }
        public int? WSPensionFund { get; set; }
        public int? WSSpecialAllowance { get; set; }
        public int? WSCompanyAllowance { get; set; }

        public decimal? WSIncentiveAllowance { get; set; }
        public decimal? WSSeniority { get; set; }
        public decimal? WSTankerAllowance { get; set; }
        public decimal? WSHousing { get; set; }
        public decimal? WSTransport { get; set; }
        public decimal? WSUtility { get; set; }
        public decimal? WSBonus { get; set; }
        public decimal? WSSeafarersPF { get; set; }
        public decimal? WSleavepayaddition { get; set; }

        public int? WSSecurityAllow { get; set; }
        public int? WSTempFuelAllow { get; set; }
        public int? WSOtherAllow { get; set; }
        public int? WSTotalWages { get; set; }
        public decimal? otrate { get; set; }
        public decimal? OTHours { get; set; }
        public decimal? BasicWages { get; set; }
        public decimal? FixedGtOT { get; set; }
        public decimal? LeavePay { get; set; }
        public decimal? UniformAllow { get; set; }
        public decimal? PensionFund { get; set; }
        public decimal? SpecialAllowance { get; set; }
        public decimal? CompanyAllowance { get; set; }

        public decimal? IncentiveAllowance { get; set; }
        public decimal? Seniority { get; set; }
        public decimal? TankerAllowance { get; set; }
        public decimal? Housing { get; set; }
        public decimal? Transport { get; set; }
        public decimal? Utility { get; set; }
        public decimal? Bonus { get; set; }
        public decimal? SeafarersPF { get; set; }
        public decimal? leavepayaddition { get; set; }

        public decimal? SecurityAllow { get; set; }
        public decimal? TempFuelAllow { get; set; }
        public decimal? OtherAllow { get; set; }
        public decimal? extraot { get; set; }
        public decimal? OtherEarnings { get; set; }
        public int? TransitDays { get; set; }
        public decimal? TransitWages { get; set; }
        public decimal? TotalWages { get; set; }
        public int? CrewListId { get; set; }
        public decimal? PrevMonthBal { get; set; }
        public decimal? Reimbursement { get; set; }
        public decimal? TotalPayable { get; set; }
        public decimal? LeaveWagesDed { get; set; }
        public decimal? CashAdvance { get; set; }
        public decimal? BondedStores { get; set; }
        public decimal? OtherDeductions { get; set; }
        public decimal? PFAmount { get; set; }
        public decimal? PFAmount10 { get; set; }
        public decimal? UDAmount { get; set; }
        public decimal? WFAmount { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Allotments { get; set; }
        public decimal? Deduction { get; set; }
        public decimal? authorityDeduction { get; set; }
        public decimal? TotalDeductions { get; set; }
        public decimal? LeaveWagesBF { get; set; }
        public decimal? LeaveWagesCF { get; set; }
        public decimal? FinalBalance { get; set; }
        public decimal? EPF { get; set; }
        public DateTime? SignOffDate { get; set; }
        public string Remarks { get; set; }
        public string Vessel { get; set; }
    }
}
