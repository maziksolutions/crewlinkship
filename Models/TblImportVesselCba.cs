using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblImportVesselCba
    {
        public int VesselCbaid { get; set; }
        public int? VesselId { get; set; }
        public int? CountryId { get; set; }
        public int? OfficerCba { get; set; }
        public int? Cbarating { get; set; }
        public DateTime? AppliedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedBy { get; set; }
    }
}
