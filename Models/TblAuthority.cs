using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblAuthority
    {
        public TblAuthority()
        {
            TblCrewCourses = new HashSet<TblCrewCourse>();
            TblCrewLicenses = new HashSet<TblCrewLicense>();
            TblCrewOtherDocuments = new HashSet<TblCrewOtherDocument>();
        }

        public int AuthorityId { get; set; }
        public string Authorities { get; set; }
        public int? CountryId { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int? CityId { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string PostCode { get; set; }
        public int? StateId { get; set; }
        public string Type { get; set; }

        public virtual TblCity City { get; set; }
        public virtual TblCountry Country { get; set; }
        public virtual TblState State { get; set; }
        public virtual ICollection<TblCrewCourse> TblCrewCourses { get; set; }
        public virtual ICollection<TblCrewLicense> TblCrewLicenses { get; set; }
        public virtual ICollection<TblCrewOtherDocument> TblCrewOtherDocuments { get; set; }
    }
}
