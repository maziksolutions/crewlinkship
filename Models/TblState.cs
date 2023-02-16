using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblState
    {
        public TblState()
        {
            TblAuthorities = new HashSet<TblAuthority>();
            TblBuilders = new HashSet<TblBuilder>();
            TblCities = new HashSet<TblCity>();
            TblCrewAddresses = new HashSet<TblCrewAddress>();
            TblCrewBankDetails = new HashSet<TblCrewBankDetail>();
            TblCrewCorrespondenceAddresses = new HashSet<TblCrewCorrespondenceAddress>();
            TblDisponentOwners = new HashSet<TblDisponentOwner>();
            TblEcdis = new HashSet<TblEcdi>();
            TblEngineModels = new HashSet<TblEngineModel>();
            TblInstitutes = new HashSet<TblInstitute>();
            TblManagers = new HashSet<TblManager>();
            TblOwners = new HashSet<TblOwner>();
            TblPrincipals = new HashSet<TblPrincipal>();
            TblVendorRegisters = new HashSet<TblVendorRegister>();
        }

        public int StateId { get; set; }
        public int CountryId { get; set; }
        public string StateName { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblCountry Country { get; set; }
        public virtual ICollection<TblAuthority> TblAuthorities { get; set; }
        public virtual ICollection<TblBuilder> TblBuilders { get; set; }
        public virtual ICollection<TblCity> TblCities { get; set; }
        public virtual ICollection<TblCrewAddress> TblCrewAddresses { get; set; }
        public virtual ICollection<TblCrewBankDetail> TblCrewBankDetails { get; set; }
        public virtual ICollection<TblCrewCorrespondenceAddress> TblCrewCorrespondenceAddresses { get; set; }
        public virtual ICollection<TblDisponentOwner> TblDisponentOwners { get; set; }
        public virtual ICollection<TblEcdi> TblEcdis { get; set; }
        public virtual ICollection<TblEngineModel> TblEngineModels { get; set; }
        public virtual ICollection<TblInstitute> TblInstitutes { get; set; }
        public virtual ICollection<TblManager> TblManagers { get; set; }
        public virtual ICollection<TblOwner> TblOwners { get; set; }
        public virtual ICollection<TblPrincipal> TblPrincipals { get; set; }
        public virtual ICollection<TblVendorRegister> TblVendorRegisters { get; set; }
    }
}
