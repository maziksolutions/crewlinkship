using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblEngineSubType
    {
        public TblEngineSubType()
        {
            TblEngineModels = new HashSet<TblEngineModel>();
        }

        public int EngineSubTypeId { get; set; }
        public int? EngineTypeId { get; set; }
        public string SubType { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblEnginetype EngineType { get; set; }
        public virtual ICollection<TblEngineModel> TblEngineModels { get; set; }
    }
}
