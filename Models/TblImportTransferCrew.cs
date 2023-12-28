using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblImportTransferCrew
    {
        public int? TransferId { get; set; }
        public int? RankId { get; set; }
        public string ActivityType { get; set; }
        public int? VesselFromId { get; set; }
        public int? VesselToId { get; set; }
        public int? PortFrom { get; set; }
        public int? PortTo { get; set; }
        public string Duration { get; set; }
        public DateTime? ActivityDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? CrewId { get; set; }
        public int? CrewListId { get; set; }
        public int? CrewListNewId { get; set; }
        public bool? IsLeaveWages { get; set; }
    }
}
