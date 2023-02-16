using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblVendorRegister
    {
        public TblVendorRegister()
        {
            TblYellowfevers = new HashSet<TblYellowfever>();
        }

        public int VendorRegisterId { get; set; }
        public string VendorName { get; set; }
        public string Code { get; set; }
        public string Preference { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Postcode { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public string PrimaryWorkCountryCode { get; set; }
        public string PrimaryWorkStateCode { get; set; }
        public string PrimaryWorkPhone { get; set; }
        public string SecondaryWorkCountryCode { get; set; }
        public string SecondaryWorkStateCode { get; set; }
        public string SecondaryWorkPhone { get; set; }
        public string PrimaryMobileCountryCode { get; set; }
        public string PrimaryMobilePhone { get; set; }
        public string SecondaryMobileCountryCode { get; set; }
        public string SecondaryMobilePhone { get; set; }
        public string SkypeId { get; set; }
        public string PrimaryEmail { get; set; }
        public string SecondaryEmail { get; set; }
        public string Picname { get; set; }
        public string MobileNo { get; set; }
        public string Services { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public int? SeaportId { get; set; }

        public virtual TblCity City { get; set; }
        public virtual TblCountry Country { get; set; }
        public virtual TblSeaport Seaport { get; set; }
        public virtual TblState State { get; set; }
        public virtual ICollection<TblYellowfever> TblYellowfevers { get; set; }
    }
}
