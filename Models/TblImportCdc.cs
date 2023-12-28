using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblImportCdc
    {
        public int? Cdcid { get; set; }
        public int? CrewId { get; set; }
        public int? CountryId { get; set; }
        public string Cdcnumber { get; set; }
        public string Place { get; set; }
        public DateTime? Doi { get; set; }
        public DateTime? Doe { get; set; }
        public string FilePath { get; set; }
        public bool? IsVerified { get; set; }
        public string VerifiedBy { get; set; }
        public DateTime? VerifyDate { get; set; }
        public string VerificationPath { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedBy { get; set; }
        public string EmailPath { get; set; }
    }
}
