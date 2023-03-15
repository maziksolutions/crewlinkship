using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace crewlinkship.ViewModel
{
    public class PortageBills
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PortageBillId { get; set; }
        public int? CrewId { get; set; }
        public int? ContractId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public int? numberofday { get; set; }
        public float? OTHours { get; set; }
        public float? extraot { get; set; }
        public float? OtherEarnings { get; set; }
        public int? TransitDays { get; set; }
        public float? TransitWages { get; set; }
        public float? TotalEarnings { get; set; }
        public float? PrevMonthBal { get; set; }
        public float? Reimbursement { get; set; }
        public float? TotalPayable { get; set; }
        public float? LeaveWagesCF { get; set; }
        public float? CashAdvance { get; set; }
        public float? BondedStores { get; set; }
        public float? OtherDeductions { get; set; }
        public float? Allotments { get; set; }
        public float? TotalDeductions { get; set; }
        public float? LeaveWagesBF { get; set; }
        public float? FinalBalance { get; set; }
        public DateTime? SignOffDate { get; set; }
        public string Remarks { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime RecDate { get; set; }
        public string createdby { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? modifieddate { get; set; }
        public int? BillStatus { get; set; }
        public int? AppliedCBA { get; set; }
        public int? bankid { get; set; }
        public int? Vesselid { get; set; }
        public decimal? Tax { get; set; }
        public decimal? UDAmount { get; set; }
        public decimal? WFAmount { get; set; }
        public bool IsTransitApply { get; set; }
        public bool ispromoted { get; set; }
        public bool IsLeaveWagesCF { get; set; }
        public string Attachment { get; set; }
        public decimal? AVC { get; set; }
        public decimal? Gratuity { get; set; }
        public decimal? IndPFAmount { get; set; }
        public bool IsAddPrevBal { get; set; }
        public bool IsHoldWageAllotment { get; set; }
 
    }
}
