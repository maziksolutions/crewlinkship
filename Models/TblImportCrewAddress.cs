using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblImportCrewAddress
    {
        public int CrewAddressId { get; set; }
        public int? CrewId { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Postcode { get; set; }
        public string CountryCode { get; set; }
        public string StateCode { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileCode { get; set; }
        public string MobileNumber { get; set; }
        public string OtherMobileCode { get; set; }
        public string OtherMobileNumber { get; set; }
        public string Email { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? AirportId { get; set; }
    }
}
