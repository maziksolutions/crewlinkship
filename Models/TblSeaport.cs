using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblSeaport
    {
        public TblSeaport()
        {
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
        public virtual ICollection<TblVendorRegister> TblVendorRegisters { get; set; }
        public virtual ICollection<TblVessel> TblVessels { get; set; }
    }
}
