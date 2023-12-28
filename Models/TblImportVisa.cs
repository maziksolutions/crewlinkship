using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblImportVisa
    {
        public int? VisaId { get; set; }
        public int? CrewId { get; set; }
        public string VisaNumber { get; set; }
        public string Place { get; set; }
        public DateTime? Doi { get; set; }
        public DateTime? Doe { get; set; }
        public string FilePath { get; set; }
        public string VisaType { get; set; }
        public string VisaSubType { get; set; }
        public int? CountryId { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedBy { get; set; }
    }
}
