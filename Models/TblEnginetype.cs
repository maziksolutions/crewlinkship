using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblEnginetype
    {
        public TblEnginetype()
        {
            TblEngineModels = new HashSet<TblEngineModel>();
            TblEngineSubTypes = new HashSet<TblEngineSubType>();
            TblVessels = new HashSet<TblVessel>();
        }

        public int EngineTypeId { get; set; }
        public string Type { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<TblEngineModel> TblEngineModels { get; set; }
        public virtual ICollection<TblEngineSubType> TblEngineSubTypes { get; set; }
        public virtual ICollection<TblVessel> TblVessels { get; set; }
    }
}
