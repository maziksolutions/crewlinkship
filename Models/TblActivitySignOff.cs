using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblActivitySignOff
    {
        public int ActivitySignOffId { get; set; }
        public int? CrewId { get; set; }
        public int? CrewListId { get; set; }
        public DateTime? SignOffDate { get; set; }
        public int? SeaportId { get; set; }
        public DateTime? EndTravelDate { get; set; }
        public DateTime? LeaveStartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public int? SignOffReasonId { get; set; }
        public DateTime? DoagivenDate { get; set; }
        public DateTime? DateOfAvailability { get; set; }
        public bool? AllowEndTravel { get; set; }
        public string DispensationApplied { get; set; }
        public string Remarks { get; set; }
        public int? ReasonDelayedId { get; set; }
        public string ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Attachment { get; set; }
        public bool? StayInHotel { get; set; }
        public bool? StayOnBoard { get; set; }
        public DateTime? StayStartDate { get; set; }
        public int? InjurySubTypeId { get; set; }
        public string InjuryType { get; set; }

        public virtual TblCrewDetail Crew { get; set; }
        public virtual TblCrewList CrewList { get; set; }
        public virtual TblSeaport Seaport { get; set; }
        public virtual TblSignOffReason SignOffReason { get; set; }
    }

    public class TblActivitySignOffVM
    {
        public int ActivitySignOffId { get; set; }
        public int CrewId { get; set; }
        public int CrewListId { get; set; }
        public string SignOffDate { get; set; }
        public string SeaportId { get; set; }
        public string EndTravelDate { get; set; }
        public string LeaveStartDate { get; set; }
        public string CompletionDate { get; set; }
        public int SignOffReasonId { get; set; }
        public string DoagivenDate { get; set; }
        public string DateOfAvailability { get; set; }
        public bool AllowEndTravel { get; set; }
        public string DispensationApplied { get; set; }
        public string Remarks { get; set; }
        public int ReasonDelayedId { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime RecDate { get; set; }
        public int CreatedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string Attachment { get; set; }
        public bool StayInHotel { get; set; }
        public bool StayOnBoard { get; set; }
        public string StayStartDate { get; set; }
        public int InjurySubTypeId { get; set; }
        public string InjuryType { get; set; }
        
    }
}
