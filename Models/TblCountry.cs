using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblCountry
    {
        public TblCountry()
        {
            TblActivitySignOns = new HashSet<TblActivitySignOn>();
            TblAssignmentsWithOthers = new HashSet<TblAssignmentsWithOther>();
            TblAuthorities = new HashSet<TblAuthority>();
            TblBuilders = new HashSet<TblBuilder>();
            TblCdcs = new HashSet<TblCdc>();
            TblCities = new HashSet<TblCity>();
            TblCrewAddresses = new HashSet<TblCrewAddress>();
            TblCrewBankDetails = new HashSet<TblCrewBankDetail>();
            TblCrewCorrespondenceAddresses = new HashSet<TblCrewCorrespondenceAddress>();
            TblCrewDetails = new HashSet<TblCrewDetail>();
            TblCrewLicenses = new HashSet<TblCrewLicense>();
            TblDisponentOwners = new HashSet<TblDisponentOwner>();
            TblEcdis = new HashSet<TblEcdi>();
            TblEngineModels = new HashSet<TblEngineModel>();
            TblInstitutes = new HashSet<TblInstitute>();
            TblIssuingAuthorities = new HashSet<TblIssuingAuthority>();
            TblManagers = new HashSet<TblManager>();
            TblOtherDocuments = new HashSet<TblOtherDocument>();
            TblOwners = new HashSet<TblOwner>();
            TblPassports = new HashSet<TblPassport>();
            TblPrincipals = new HashSet<TblPrincipal>();
            TblSeaports = new HashSet<TblSeaport>();
            TblStates = new HashSet<TblState>();
            TblVendorRegisters = new HashSet<TblVendorRegister>();
            TblVesselCbas = new HashSet<TblVesselCba>();
            TblVessels = new HashSet<TblVessel>();
            TblVisas = new HashSet<TblVisa>();
        }

        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string Nationality { get; set; }
        public string Iso3 { get; set; }
        public string StdCodes { get; set; }
        public string Currency { get; set; }
        public string CurrencyName { get; set; }
        public string SortName { get; set; }
        public int? Priority { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CurrencyPriority { get; set; }

        public virtual ICollection<TblActivitySignOn> TblActivitySignOns { get; set; }
        public virtual ICollection<TblAssignmentsWithOther> TblAssignmentsWithOthers { get; set; }
        public virtual ICollection<TblAuthority> TblAuthorities { get; set; }
        public virtual ICollection<TblBuilder> TblBuilders { get; set; }
        public virtual ICollection<TblCdc> TblCdcs { get; set; }
        public virtual ICollection<TblCity> TblCities { get; set; }
        public virtual ICollection<TblCrewAddress> TblCrewAddresses { get; set; }
        public virtual ICollection<TblCrewBankDetail> TblCrewBankDetails { get; set; }
        public virtual ICollection<TblCrewCorrespondenceAddress> TblCrewCorrespondenceAddresses { get; set; }
        public virtual ICollection<TblCrewDetail> TblCrewDetails { get; set; }
        public virtual ICollection<TblCrewLicense> TblCrewLicenses { get; set; }
        public virtual ICollection<TblDisponentOwner> TblDisponentOwners { get; set; }
        public virtual ICollection<TblEcdi> TblEcdis { get; set; }
        public virtual ICollection<TblEngineModel> TblEngineModels { get; set; }
        public virtual ICollection<TblInstitute> TblInstitutes { get; set; }
        public virtual ICollection<TblIssuingAuthority> TblIssuingAuthorities { get; set; }
        public virtual ICollection<TblManager> TblManagers { get; set; }
        public virtual ICollection<TblOtherDocument> TblOtherDocuments { get; set; }
        public virtual ICollection<TblOwner> TblOwners { get; set; }
        public virtual ICollection<TblPassport> TblPassports { get; set; }
        public virtual ICollection<TblPrincipal> TblPrincipals { get; set; }
        public virtual ICollection<TblSeaport> TblSeaports { get; set; }
        public virtual ICollection<TblState> TblStates { get; set; }
        public virtual ICollection<TblVendorRegister> TblVendorRegisters { get; set; }
        public virtual ICollection<TblVesselCba> TblVesselCbas { get; set; }
        public virtual ICollection<TblVessel> TblVessels { get; set; }
        public virtual ICollection<TblVisa> TblVisas { get; set; }
    }
}
