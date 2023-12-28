using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblImportAssignmentsWithOther
    {
        public int OtherAssignmentsId { get; set; }
        public int? CrewId { get; set; }
        public string CompanyName { get; set; }
        public int? RankId { get; set; }
        public DateTime? SignonDate { get; set; }
        public DateTime? SignoffDate { get; set; }
        public string VesselName { get; set; }
        public int? ShipId { get; set; }
        public int? EngineModelId { get; set; }
        public int? SignOffReasonId { get; set; }
        public string Dwt { get; set; }
        public string Grt { get; set; }
        public string Kw { get; set; }
        public string Propulsion { get; set; }
        public string Imo { get; set; }
        public string TotalDays { get; set; }
        public int? CountryId { get; set; }
        public int? ManagerId { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? SeaportId { get; set; }
        public int? CreatedBy { get; set; }
    }
}
