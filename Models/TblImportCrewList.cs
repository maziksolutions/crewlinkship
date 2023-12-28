using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblImportCrewList
    {
        public int? CrewListId { get; set; }
        public int? RankId { get; set; }
        public int? VesselId { get; set; }
        public int? CrewId { get; set; }
        public DateTime? SignOnDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int? Reliever1 { get; set; }
        public int? Reliever2 { get; set; }
        public string ReptriationPort { get; set; }
        public string EngagementPort { get; set; }
        public string Er { get; set; }
        public string Ermonth { get; set; }
        public string Status { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool? IsSignOff { get; set; }
        public string ReplacedWith { get; set; }
        public DateTime? OldDueDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsPromoted { get; set; }
        public int? ActivityCode { get; set; }
        public int? ReliverRankId { get; set; }
        public int? PlanActivityCode { get; set; }
    }
}
