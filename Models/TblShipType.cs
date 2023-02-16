using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblShipType
    {
        public TblShipType()
        {
            TblVessels = new HashSet<TblVessel>();
        }

        public int ShipId { get; set; }
        public string ShipCategory { get; set; }
        public string Type { get; set; }
        public string ShipRange { get; set; }
        public string ShipRangeTo { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<TblVessel> TblVessels { get; set; }
    }
}
