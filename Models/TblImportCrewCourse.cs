using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblImportCrewCourse
    {
        public int? CrewCoursesId { get; set; }
        public int? CrewId { get; set; }
        public int? CourseId { get; set; }
        public int? InstituteId { get; set; }
        public int? AuthorityId { get; set; }
        public string Course { get; set; }
        public string CertificateNumber { get; set; }
        public string PlaceOfIssue { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool? IsVerified { get; set; }
        public string LimitationRemarks { get; set; }
        public string Attachment { get; set; }
        public string Verification { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string VerifiedBy { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public int? CreatedBy { get; set; }
    }
}
