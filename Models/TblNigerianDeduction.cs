using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblNigerianDeduction
    {
        public int NigDeductionId { get; set; }
        public int? Cbaid { get; set; }
        public int? RankId { get; set; }
        public string DeductionType { get; set; }
        public string Deduction { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }

        public virtual TblCba Cba { get; set; }
        public virtual TblRankRegister Rank { get; set; }
    }
}
