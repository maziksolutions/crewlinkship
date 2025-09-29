        using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblImportMidMonthAllotment
    {
        public int? MidMonthAllotmentId { get; set; }
        public int? CrewId { get; set; }
        public int? CrewListId { get; set; }
        public int? ContractId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public double? Allotments { get; set; }
        public double? FinalBalance { get; set; }
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
    }
}
