using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblCrewBankDetail
    {
        public int CrewBankDetailsId { get; set; }
        public int? CrewId { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public string AccountType { get; set; }
        public string Beneficiary { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string SwiftCode { get; set; }
        public string BankAddress { get; set; }
        public string BankAddress2 { get; set; }
        public string Postcode { get; set; }
        public string Ifsccode { get; set; }
        public string SortCode { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string Attachment { get; set; }
        public int? CreatedBy { get; set; }

        public virtual TblCity City { get; set; }
        public virtual TblCountry Country { get; set; }
        public virtual TblCrewDetail Crew { get; set; }
        public virtual TblState State { get; set; }
    }
}
