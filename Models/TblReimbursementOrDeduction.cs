using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblReimbursementOrDeduction
    {
        public int ReimbursementOrDeductionId { get; set; }
        public int? ContractId { get; set; }
        public string Component { get; set; }
        public string Amount { get; set; }
        public string Type { get; set; }
        public DateTime? RecDate { get; set; }
        public bool? IsDeleted { get; set; }
        public string Payable { get; set; }
    }
}
