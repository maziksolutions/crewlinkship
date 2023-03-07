using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace crewlinkship.ViewModel
{
    public class PortageBillVM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int pid { get; set; }
        public int? PortageBillId { get; set; }
        public int? CrewId { get; set; }
        public int? ContractId { get; set; }
        public int? numberofday { get; set; }
        public string CrewName { get; set; }
        public string RankName { get; set; }
        public string cdcno { get; set; }
        public string Nationality { get; set; }
        public int? Rankid { get; set; }
        public int? AppliedCBA { get; set; }
        public DateTime? Paydate { get; set; }
        public DateTime? SignOnDate { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public int? TotalDays { get; set; }
        public string Currency { get; set; }
        public decimal? BasicWages { get; set; }
        public decimal? FixedGtOT { get; set; }
        public decimal? SecurityAllow { get; set; }
        public decimal? LeavePay { get; set; }
        public decimal? UniformAllow { get; set; }
        public decimal? PensionFund { get; set; }
        public decimal? OtherAllow { get; set; }
        public decimal? TotalWages { get; set; }
        public int? CrewListId { get; set; }
        public decimal? OTHours { get; set; }
        public decimal? OtherEarnings { get; set; }
        public int? TransitDays { get; set; }
        public decimal? TransitWages { get; set; }
        public decimal? CashAdvance { get; set; }
        public decimal? BondedStores { get; set; }
        public decimal? OtherDeductions { get; set; }
        public int? Allotments { get; set; }
        public int? PFAmount { get; set; }
        public int? UDAmount { get; set; }
        public int? WFAmount { get; set; }
        public int? Deduction { get; set; }
        public decimal? authorityDeduction { get; set; }
        public decimal? Reimbursement { get; set; }
        public decimal? otrate { get; set; }
        public decimal? extraot { get; set; }
        public decimal? LeaveWagesBF { get; set; }
        public decimal? TotalDeductions { get; set; }
        public decimal? TotalPayable { get; set; }
        public decimal? PrevMonthBal { get; set; }
        public decimal? FinalBalance { get; set; }
        public DateTime? SignOffDate { get; set; }
        public string Remarks { get; set; }
        public string Vessel { get; set; }
        public int? Vesselid { get; set; }
        public int? bankid { get; set; }
        public int? Tax { get; set; }
        public int? UDdeduction { get; set; }
        public int? WFdeduction { get; set; }
        public int? WHTdeduction { get; set; }
        public int? AllTotalDays { get; set; }
        public DateTime? createddate { get; set; }
        public DateTime? modifieddate { get; set; }
        public string createdby { get; set; }
        public string modifyby { get; set; }
        public string ispromoted { get; set; }
        public string signoffreason { get; set; }
        public int Islocked { get; set; }
        public string Attachment { get; set; }
        public decimal? Gratuity { get; set; }
        public decimal? IndPFAmount { get; set; }
        public decimal? AVC { get; set; }
        public string bowRequest { get; set; }
        public bool IsAddPrevBal { get; set; }
        public decimal? MidMonthAllotment { get; set; }
    }
    public class PortageBillSignoffVM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int pid { get; set; }
        public int? PortageBillId { get; set; }
        public int? CrewId { get; set; }
        public int? ContractId { get; set; }
        public int? numberofday { get; set; }
        public string CrewName { get; set; }
        public string RankName { get; set; }
        public string cdcno { get; set; }
        public string Nationality { get; set; }
        public int? Rankid { get; set; }
        public int? AppliedCBA { get; set; }
        public DateTime? Paydate { get; set; }
        public DateTime? SignOnDate { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public int? TotalDays { get; set; }
        public string Currency { get; set; }
        public decimal? BasicWages { get; set; }
        public decimal? FixedGtOT { get; set; }
        public decimal? SecurityAllow { get; set; }
        public decimal? LeavePay { get; set; }
        //public decimal? SubsitenceAllow { get; set; }
        public decimal? UniformAllow { get; set; }
        public decimal? PensionFund { get; set; }
        public decimal? OtherAllow { get; set; }
        public decimal? TotalWages { get; set; }
        public int? CrewListId { get; set; }
        public decimal? OTHours { get; set; }
        public decimal? OtherEarnings { get; set; }
        public int? TransitDays { get; set; }
        public decimal? TransitWages { get; set; }
        public decimal? CashAdvance { get; set; }
        public decimal? BondedStores { get; set; }
        public decimal? OtherDeductions { get; set; }
        public int? Allotments { get; set; }
        public int? PFAmount { get; set; }
        public int? UDAmount { get; set; }
        public int? WFAmount { get; set; }
        public int? Deduction { get; set; }
        public decimal? authorityDeduction { get; set; }
        public decimal? Reimbursement { get; set; }
        public decimal? otrate { get; set; }
        public decimal? extraot { get; set; }
        public decimal? LeaveWagesBF { get; set; }
        public decimal? TotalDeductions { get; set; }
        public decimal? TotalPayable { get; set; }
        public decimal? PrevMonthBal { get; set; }
        public decimal? FinalBalance { get; set; }
        public DateTime? SignOffDate { get; set; }
        public string Remarks { get; set; }
        public string Vessel { get; set; }
        public int? Vesselid { get; set; }
        public int? bankid { get; set; }
        public decimal? Tax { get; set; }
        public int? UDdeduction { get; set; }
        public int? WFdeduction { get; set; }
        public int? WHTdeduction { get; set; }
        //
        public DateTime? createddate { get; set; }
        public DateTime? modifieddate { get; set; }
        public string createdby { get; set; }
        public string modifyby { get; set; }
        public int? AllTotalDays { get; set; }
        public string signoffreason { get; set; }
        public string ispromoted { get; set; }
        public int Islocked { get; set; }
        public int IsLeaveWagesCF { get; set; }
        public string Attachment { get; set; }
        public decimal? Gratuity { get; set; }
        public decimal? IndPFAmount { get; set; }
        public decimal? AVC { get; set; }
        public bool IsAddPrevBal { get; set; }
        public decimal? MidMonthAllotment { get; set; }
    }

}
