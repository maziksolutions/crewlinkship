using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblVesselCba
    {
        public int VesselCbaid { get; set; }
        public int? VesselId { get; set; }
        public int? CountryId { get; set; }

        [ForeignKey("OffCBA")]
        public int? OfficerCba { get; set; }
        [ForeignKey("RatingCBA")]
        public int? Cbarating { get; set; }
        public DateTime? AppliedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedBy { get; set; }

        public virtual TblCba OffCBA { get; set; }

        public virtual TblCba RatingCBA { get; set; }
        public virtual TblCountry Country { get; set; }
        public virtual TblVessel Vessel { get; set; }
    }
}
