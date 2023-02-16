using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblOtherDocument
    {
        public TblOtherDocument()
        {
            TblCrewOtherDocuments = new HashSet<TblCrewOtherDocument>();
        }

        public int DocumentId { get; set; }
        public int? CountryId { get; set; }
        public string DocumentName { get; set; }
        public string Priority { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public bool? Joiner { get; set; }
        public bool? IsSunStoneDoc { get; set; }
        public bool? IsCheckDoc { get; set; }

        public virtual TblCountry Country { get; set; }
        public virtual ICollection<TblCrewOtherDocument> TblCrewOtherDocuments { get; set; }
    }
}
