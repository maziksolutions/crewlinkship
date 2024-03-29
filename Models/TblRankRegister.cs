﻿using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblRankRegister
    {
        public TblRankRegister()
        {
            TblActivitySignOns = new HashSet<TblActivitySignOn>();
            TblAssignmentsWithOthers = new HashSet<TblAssignmentsWithOther>();
            TblAssignmentsWithOurs = new HashSet<TblAssignmentsWithOur>();
            TblCrewDetailPlanRanks = new HashSet<TblCrewDetail>();
            TblCrewDetailRanks = new HashSet<TblCrewDetail>();
            TblCrewListRanks = new HashSet<TblCrewList>();
            TblCrewListReliverRanks = new HashSet<TblCrewList>();
            TblNigerianDeductions = new HashSet<TblNigerianDeduction>();
            TblOverTimes = new HashSet<TblOverTime>();
            TblPfrates = new HashSet<TblPfrate>();
            TblTransferCrews = new HashSet<TblTransferCrew>();
            TblWageStructures = new HashSet<TblWageStructure>();
        }

        public int RankId { get; set; }
        public string RankName { get; set; }
        public string Code { get; set; }
        public string Level { get; set; }
        public int CrewSort { get; set; }
        public string GroupRank { get; set; }
        public string Department { get; set; }
        public string OfficeCrew { get; set; }
        public string Level1 { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int PortageSort { get; set; }

        public virtual ICollection<TblActivitySignOn> TblActivitySignOns { get; set; }
        public virtual ICollection<TblAssignmentsWithOther> TblAssignmentsWithOthers { get; set; }
        public virtual ICollection<TblAssignmentsWithOur> TblAssignmentsWithOurs { get; set; }
        public virtual ICollection<TblCrewDetail> TblCrewDetailPlanRanks { get; set; }
        public virtual ICollection<TblCrewDetail> TblCrewDetailRanks { get; set; }
        public virtual ICollection<TblCrewList> TblCrewListRanks { get; set; }
        public virtual ICollection<TblCrewList> TblCrewListReliverRanks { get; set; }
        public virtual ICollection<TblNigerianDeduction> TblNigerianDeductions { get; set; }
        public virtual ICollection<TblOverTime> TblOverTimes { get; set; }
        public virtual ICollection<TblPfrate> TblPfrates { get; set; }
        public virtual ICollection<TblTransferCrew> TblTransferCrews { get; set; }
        public virtual ICollection<TblWageStructure> TblWageStructures { get; set; }
    }
}
