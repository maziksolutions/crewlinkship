using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblCrewCorrespondence
    {
        public int CrewCorrespondenceId { get; set; }
        public int? CrewId { get; set; }
        public int? CorrespondenceId { get; set; }
        public int? RemarksId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public DateTime? Date { get; set; }
        public string Attachment { get; set; }
        public string Message { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblCrewDetail Crew { get; set; }
    }
}
