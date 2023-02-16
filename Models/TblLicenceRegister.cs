using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblLicenceRegister
    {
        public TblLicenceRegister()
        {
            TblCrewLicenses = new HashSet<TblCrewLicense>();
        }

        public int LicenceId { get; set; }
        public string LicenceName { get; set; }
        public string StcwCode { get; set; }
        public string Level { get; set; }
        public string GrtKw { get; set; }
        public string Type { get; set; }
        public string Authority { get; set; }
        public bool? ExpiryApplicable { get; set; }
        public bool? RenewalRequired { get; set; }
        public bool? AuthenticationRequired { get; set; }
        public string StcwRemarks { get; set; }
        public string Group { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<TblCrewLicense> TblCrewLicenses { get; set; }
    }
}
