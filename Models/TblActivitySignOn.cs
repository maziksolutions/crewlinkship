using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblActivitySignOn
    {
        public int ActivitySignOnId { get; set; }
        public int? CrewId { get; set; }
        public int? ContractId { get; set; }
        public int? VesselId { get; set; }
        public int? CountryId { get; set; }
        public int? SeaportId { get; set; }
        public int? RankId { get; set; }
        public int? SignOnReasonId { get; set; }
        public int? ReliveesCrewListId { get; set; }
        public string Contract { get; set; }
        public DateTime? ExpectedSignOnDate { get; set; }
        public string Duration { get; set; }
        public DateTime? ReliefDate { get; set; }
        public DateTime? ExpectedTravelDate { get; set; }
        public string ExtraCrewOnBoard { get; set; }
        public int? ExtraCrewReasonId { get; set; }
        public string ExtraApprovedBy { get; set; }
        public string DocsValidityCheckPeriod { get; set; }
        public bool? AllowBeginTravel { get; set; }
        public bool? PreJoiningMedicals { get; set; }
        public bool? Appraisal { get; set; }
        public bool? OwnerWage { get; set; }
        public string Remarks { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsSignon { get; set; }
        public DateTime? RecDate { get; set; }
        public int? CreatedBy { get; set; }

        public virtual TblCountry Country { get; set; }
        public virtual TblCrewDetail Crew { get; set; }
        public virtual TblRankRegister Rank { get; set; }
        public virtual TblSeaport Seaport { get; set; }
        public virtual TblSignOnReason SignOnReason { get; set; }
    }

    public class TblActivitySignOnVM
    {
        public int ActivitySignOnId { get; set; }
        public int? CrewId { get; set; }
        public int? ContractId { get; set; }
        public int? VesselId { get; set; }
        public int? CountryId { get; set; }
        public int? SeaportId { get; set; }
        public int? RankId { get; set; }
        public int? SignOnReasonId { get; set; }
        public int? ReliveesCrewListId { get; set; }
        public string Contract { get; set; }
        public string ExpectedSignOnDate { get; set; }
        public string Duration { get; set; }
        public string ReliefDate { get; set; }
        public string ExpectedTravelDate { get; set; }
        public string ExtraCrewOnBoard { get; set; }
        public int? ExtraCrewReasonId { get; set; }
        public string ExtraApprovedBy { get; set; }
        public string DocsValidityCheckPeriod { get; set; }
        public bool? AllowBeginTravel { get; set; }
        public bool? PreJoiningMedicals { get; set; }
        public bool? Appraisal { get; set; }
        public bool? OwnerWage { get; set; }
        public string Remarks { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsSignon { get; set; }
        public string RecDate { get; set; }
        public int? CreatedBy { get; set; }
    }
}


