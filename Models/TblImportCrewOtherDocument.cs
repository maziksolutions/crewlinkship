using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblImportCrewOtherDocument
    {
        public int? CrewOtherDocumentsId { get; set; }
        public int? CrewId { get; set; }
        public int? DocumentId { get; set; }
        public int? AuthorityId { get; set; }
        public string DocumentNo { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? ExtendedDate { get; set; }
        public string PlaceOfIssue { get; set; }
        public string Attachment { get; set; }
        public string Remarks { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public int? CreatedBy { get; set; }
    }
}
