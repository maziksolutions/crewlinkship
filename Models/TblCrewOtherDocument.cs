using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblCrewOtherDocument
    {
        public int CrewOtherDocumentsId { get; set; }
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

        public virtual TblAuthority Authority { get; set; }
        public virtual TblCrewDetail Crew { get; set; }
        public virtual TblOtherDocument Document { get; set; }
    }
    public class TblCrewOtherDocumentVM
    {
        public int CrewOtherDocumentsId { get; set; }
        public int CrewId { get; set; }
        public int DocumentId { get; set; }
        public int AuthorityId { get; set; }
        public string DocumentNo { get; set; }
        public string IssueDate { get; set; }
        public string ExpiryDate { get; set; }
        public string ExtendedDate { get; set; }
        public string PlaceOfIssue { get; set; }
        public string Attachment { get; set; }
        public string Remarks { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string RecDate { get; set; }
        public int CreatedBy { get; set; }

    }
}
