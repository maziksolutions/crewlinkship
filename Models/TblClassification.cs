using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblClassification
    {
        public TblClassification()
        {
            TblVessels = new HashSet<TblVessel>();
        }

        public int ClassificationId { get; set; }
        public string Classifications { get; set; }
        public string ShortCode { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<TblVessel> TblVessels { get; set; }
    }
}
