using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblCourseRegister
    {
        public TblCourseRegister()
        {
            TblCrewCourses = new HashSet<TblCrewCourse>();
        }

        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public string CourseType { get; set; }
        public string Reference { get; set; }
        public string Level { get; set; }
        public string Method { get; set; }
        public string RankId { get; set; }
        public string Group { get; set; }
        public bool? ExpiryApplicable { get; set; }
        public bool? RenewalRequired { get; set; }
        public bool? AuthenticationRequired { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<TblCrewCourse> TblCrewCourses { get; set; }
    }
}
