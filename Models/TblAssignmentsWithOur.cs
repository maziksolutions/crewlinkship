using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblAssignmentsWithOur
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
        public int? CrewListId { get; set; }
        public int? ShipTypeShipId { get; set; }
        public int? EngineModelId { get; set; }
        public int? SignOffPortId { get; set; }
        public int? SignOnPortId { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public DateTime? TravelDate { get; set; }
        public int? InjurySubTypeId { get; set; }
        public string InjuryType { get; set; }
        public int? VendorRegisterId { get; set; }

        public virtual TblCrewList CrewList { get; set; }
        public virtual TblEngineModel EngineModel { get; set; }
        public virtual TblRankRegister Rank { get; set; }
        public virtual TblShipType ShipTypeShip { get; set; }
        public virtual TblSignOffReason SignOffReason { get; set; }
        public virtual TblVendorRegister VendorRegister { get; set; }
        public virtual TblVessel Vessel { get; set; }
    }
}
