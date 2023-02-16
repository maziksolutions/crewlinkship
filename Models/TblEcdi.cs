using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblEcdi
    {
        public TblEcdi()
        {
            TblVesselEcdisid1Navigations = new HashSet<TblVessel>();
            TblVesselEcdisid2Navigations = new HashSet<TblVessel>();
            TblVesselEcdisid3Navigations = new HashSet<TblVessel>();
        }

        public int Ecdisid { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public string Model { get; set; }
        public string Maker { get; set; }
        public string Street { get; set; }
        public string Area { get; set; }
        public string PostCode { get; set; }
        public string CountryCode { get; set; }
        public string StateCode { get; set; }
        public string LandlineNo { get; set; }
        public string MobileCode { get; set; }
        public string MobileNo { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblCity City { get; set; }
        public virtual TblCountry Country { get; set; }
        public virtual TblState State { get; set; }
        public virtual ICollection<TblVessel> TblVesselEcdisid1Navigations { get; set; }
        public virtual ICollection<TblVessel> TblVesselEcdisid2Navigations { get; set; }
        public virtual ICollection<TblVessel> TblVesselEcdisid3Navigations { get; set; }
    }
}
