using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblBowRequest
    {
        public int BowRequestId { get; set; }
        public string BowType { get; set; }
        public string LeaveWagesCarry { get; set; }
        public string Status { get; set; }
        public int? CrewId { get; set; }
        public int? CrewListId { get; set; }
        public string CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime RecDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int VesselId { get; set; }
        public string Notification { get; set; }

        public virtual TblCrewDetail Crew { get; set; }
    }
}
