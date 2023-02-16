using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblCbaUnion
    {
        public TblCbaUnion()
        {
            TblCbas = new HashSet<TblCba>();
        }

        public int UnionId { get; set; }
        public string UnionName { get; set; }
        public string ModifiedBy { get; set; }
        public string RankGroup { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }

        public virtual ICollection<TblCba> TblCbas { get; set; }
    }
}
