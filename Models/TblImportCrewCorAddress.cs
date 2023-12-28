using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblImportCrewCorAddress
    {
        public int CrewAddressId { get; set; }
        public int? CrewId { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public int? AirportId { get; set; }
        public string Caddress1 { get; set; }
        public string Caddress2 { get; set; }
        public string Postcode { get; set; }
        public bool? SameAddress { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public int? CreatedBy { get; set; }
    }
}
