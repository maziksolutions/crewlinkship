using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblImportAssignmentsWithOur
    {
        public int OurAssignmentsId { get; set; }
        public int? CrewId { get; set; }
        public string CompanyName { get; set; }
        public int? RankId { get; set; }
        public DateTime? SignonDate { get; set; }
        public DateTime? SignoffDate { get; set; }
        public int? Duration { get; set; }
        public int? VesselId { get; set; }
        public int? SignOffReasonId { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public int? SignOffPortId { get; set; }
        public int? SignOnPortId { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public DateTime? TravelDate { get; set; }
        public int? InjurySubTypeId { get; set; }
        public string InjuryType { get; set; }
        public int? VendorRegisterId { get; set; }
    }
}
