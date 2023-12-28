using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblPfrate
    {
        public int PfrateId { get; set; }
        public int RankId { get; set; }
        public int Cbaid { get; set; }
        public decimal Pfamount { get; set; }
        public decimal GratuityAmount { get; set; }
        public decimal Avcamount { get; set; }
        public int CreatedBy { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public bool? IsLocked { get; set; }

        public virtual TblCba Cba { get; set; }
        public virtual TblRankRegister Rank { get; set; }
    }
}
