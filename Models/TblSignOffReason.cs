using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblSignOffReason
    {
        public int SignOffReasonId { get; set; }
        public string Code { get; set; }
        public string Reason { get; set; }
        public string Group { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
    }
}
