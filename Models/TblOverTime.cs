using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblOverTime
    {
        public int Otid { get; set; }
        public int RankId { get; set; }
        public string Otrate { get; set; }
        public int Cbaid { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }

        public virtual TblRankRegister Rank { get; set; }
    }
}
