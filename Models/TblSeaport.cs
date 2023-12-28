using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblSeaport
    {
        public TblSeaport()
        {
            TblActivitySignOffs = new HashSet<TblActivitySignOff>();
            TblActivitySignOns = new HashSet<TblActivitySignOn>();
            TblAssignmentsWithOthers = new HashSet<TblAssignmentsWithOther>();
            TblContracts = new HashSet<TblContract>();
            TblVendorRegisters = new HashSet<TblVendorRegister>();
            TblVessels = new HashSet<TblVessel>();
        }

        public int SeaportId { get; set; }
        public int? CountryId { get; set; }
        public string SeaportName { get; set; }
        public string Code { get; set; }
        public bool? IsDeleted { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime RecDate { get; set; }

        public virtual TblCountry Country { get; set; }
        public virtual ICollection<TblActivitySignOff> TblActivitySignOffs { get; set; }
        public virtual ICollection<TblActivitySignOn> TblActivitySignOns { get; set; }
        public virtual ICollection<TblAssignmentsWithOther> TblAssignmentsWithOthers { get; set; }
        public virtual ICollection<TblContract> TblContracts { get; set; }
        public virtual ICollection<TblVendorRegister> TblVendorRegisters { get; set; }
        public virtual ICollection<TblVessel> TblVessels { get; set; }
    }
}
