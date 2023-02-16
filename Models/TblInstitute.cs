using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblInstitute
    {
        public TblInstitute()
        {
            TblCrewCourses = new HashSet<TblCrewCourse>();
        }

        public int InstituteId { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public string InstituteName { get; set; }
        public string Street { get; set; }
        public string Area { get; set; }
        public string PostCode { get; set; }
        public string CountryCode { get; set; }
        public string StateCode { get; set; }
        public string Phone { get; set; }
        public string Code { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblCity City { get; set; }
        public virtual TblCountry Country { get; set; }
        public virtual TblState State { get; set; }
        public virtual ICollection<TblCrewCourse> TblCrewCourses { get; set; }
    }
}
