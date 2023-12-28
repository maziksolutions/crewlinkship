using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblImportYellowfever
    {
        public int? YellowFeverId { get; set; }
        public int? CrewId { get; set; }
        public string Reference { get; set; }
        public string Place { get; set; }
        public int? VendorRegisterId { get; set; }
        public DateTime? VaccineDate { get; set; }
        public string VaccineBatch { get; set; }
        public string Attachment { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedBy { get; set; }
    }
}
