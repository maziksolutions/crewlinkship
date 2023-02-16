using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblWageStructure
    {
        public int WageStructureId { get; set; }
        public int RankId { get; set; }
        public int? WageId { get; set; }
        public string WageAmount { get; set; }
        public int Cbaid { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string SubCode { get; set; }

        public virtual TblCba Cba { get; set; }
        public virtual TblRankRegister Rank { get; set; }
        public virtual TblWageComponent Wage { get; set; }
    }
}
