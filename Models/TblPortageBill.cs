using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblPortageBill
    {
        public int PortageBillId { get; set; }
        public int? CrewId { get; set; }
        public int? CrewListId { get; set; }
        public int? ContractId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public int? Days { get; set; }
        public double? Othours { get; set; }
        public double? ExtraOt { get; set; }
        public double? OtherEarnings { get; set; }
        public int? TransitDays { get; set; }
        public double? TransitWages { get; set; }
        public double? TotalEarnings { get; set; }
        public double? PrevMonthBal { get; set; }
        public double? Reimbursement { get; set; }
        public double? TotalPayable { get; set; }
        public double? LeaveWagesCf { get; set; }
        public double? CashAdvance { get; set; }
        public double? BondedStores { get; set; }
        public double? OtherDeductions { get; set; }
        public double? Allotments { get; set; }
        public double? TotalDeductions { get; set; }
        public double? LeaveWagesBf { get; set; }
        public double? FinalBalance { get; set; }
        public DateTime? SignOffDate { get; set; }
        [MaxLength(10000)]
        public string Remarks { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? AppliedCba { get; set; }
        public int? BillStatus { get; set; }
        public int? BankId { get; set; }
        public int? Vesselid { get; set; }
        public decimal? Udamount { get; set; }
        public decimal? Wfamount { get; set; }
        public decimal? Tax { get; set; }
        public bool? IsTransitApply { get; set; }
        public bool? IsPromoted { get; set; }
        public bool? IsLeaveWagesCf { get; set; }
        public string Attachment { get; set; }
        public decimal? IndPfamount { get; set; }
        public decimal? Gratuity { get; set; }
        public decimal? Avc { get; set; }
        public bool IsAddPrevBal { get; set; }
        public bool? IsHoldWageAllotment { get; set; }

        public virtual TblCba AppliedCbaNavigation { get; set; }
        public virtual TblCrewDetail Crew { get; set; }
        public virtual TblCrewList CrewList { get; set; }
        public virtual TblContract Contract { get; set; }
    }
    public class TblPortageBillVM
    {
        public int VesselPortId { get; set; }
        public int? CrewId { get; set; }
        public int? CrewListId { get; set; }
        public int? ContractId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int? Days { get; set; }
        public double? Othours { get; set; }
        public double? ExtraOt { get; set; }
        public double? OtherEarnings { get; set; }
        public int? TransitDays { get; set; }
        public double? TransitWages { get; set; }
        public double? TotalEarnings { get; set; }
        public double? PrevMonthBal { get; set; }
        public double? Reimbursement { get; set; }
        public double? TotalPayable { get; set; }
        public double? LeaveWagesCf { get; set; }
        public double? CashAdvance { get; set; }
        public double? BondedStores { get; set; }
        public double? OtherDeductions { get; set; }
        public double? Allotments { get; set; }
        public double? TotalDeductions { get; set; }
        public double? LeaveWagesBf { get; set; }
        public double? FinalBalance { get; set; }
        public string SignOffDate { get; set; }
        public string Remarks { get; set; }
        public bool? IsDeleted { get; set; }
        public string RecDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public int? AppliedCba { get; set; }
        public int? BillStatus { get; set; }
        public int? BankId { get; set; }
        public int? Vesselid { get; set; }
        public decimal? Udamount { get; set; }
        public decimal? Wfamount { get; set; }
        public decimal? Tax { get; set; }
        public bool? IsTransitApply { get; set; }
        public bool? IsPromoted { get; set; }
        public bool? IsLeaveWagesCf { get; set; }
        public string Attachment { get; set; }
        public decimal? IndPfamount { get; set; }
        public decimal? Gratuity { get; set; }
        public decimal? Avc { get; set; }
        public bool IsAddPrevBal { get; set; }
        public bool? IsHoldWageAllotment { get; set; }

    }
}
