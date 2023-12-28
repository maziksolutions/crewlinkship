using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblImportPassport
    {
        public int PassportId { get; set; }
        public string PassportNumber { get; set; }
        public int CrewId { get; set; }
        public string Place { get; set; }
        public DateTime Doi { get; set; }
        public DateTime Doe { get; set; }
        public string FilePath { get; set; }
        public string BlankPages { get; set; }
        public string Ecnr { get; set; }
        public int? CountryId { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsVerified { get; set; }
        public string VerifiedBy { get; set; }
        public DateTime? VerifyDate { get; set; }
        public string VerificationPath { get; set; }
        public string EmailPath { get; set; }
        public int? CreatedBy { get; set; }
    }
}
