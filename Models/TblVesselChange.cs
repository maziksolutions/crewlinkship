using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblVesselChange
    {
        public int VesselChangeId { get; set; }
        public int VesselId { get; set; }
        public bool? IsName { get; set; }
        public bool? IsFlag { get; set; }
        public bool? IsManager { get; set; }
        public bool? Isowner { get; set; }
        public string NewVesselName { get; set; }
        public int? NewManagerId { get; set; }
        public int? NewFlagId { get; set; }
        public int? NewOwnerId { get; set; }
        public bool? IsWagesCarriedForward { get; set; }
        public bool? IsSameCba { get; set; }
        public bool? IsSameParticulars { get; set; }
        public DateTime? ExpectedTakeOverDate { get; set; }
        public int? ExpectedTakeOverPort { get; set; }
        public bool? IsActivityReversed { get; set; }
        public int? NewVesselId { get; set; }
        public string Remarks { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public int? CreatedBy { get; set; }

        public virtual TblVessel NewVessel { get; set; }
        public virtual TblVessel Vessel { get; set; }
    }
}
