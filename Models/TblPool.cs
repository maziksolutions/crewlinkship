using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblPool
    {
        public TblPool()
        {
            TblCrewDetails = new HashSet<TblCrewDetail>();
            TblVessels = new HashSet<TblVessel>();
        }

        public int PoolId { get; set; }
        public string PoolName { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }

        public virtual ICollection<TblCrewDetail> TblCrewDetails { get; set; }
        public virtual ICollection<TblVessel> TblVessels { get; set; }
    }
}
