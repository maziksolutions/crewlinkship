using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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

    public class TblImportBudgetCode
    {
        public int? BudgetCodeId { get; set; }
        public string BudgetCodeDescription { get; set; }
        public string BudgetCodes { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
    }
    public class TblImportBudgetSubCode
    {
        public int? SubCodeId { get; set; }
        public int BudgetCodeId { get; set; }
        public int SubCode { get; set; }
        public string SubBudget { get; set; }
        public string Description { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
    }
    public class TblImportWageComponent
    {
        public int? WageId { get; set; }
        public int? BudgetCodeId { get; set; }
        public int? SubCodeId { get; set; }
        public string CalculationBasis { get; set; }
        public string PayableBasis { get; set; }
        public string IncludedOnboard { get; set; }
        public string Earning { get; set; }
        public string IsCBA { get; set; }
        public bool? IsShowAll { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ColumnConfigId { get; set; }
    }

    public class TblImportWageStructure
    {
        public int? WageStructureId { get; set; }
        public int RankId { get; set; }
        public int? WageId { get; set; }
        public string WageAmount { get; set; }
        public int CBAId { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
    }
    public class TblImportCBAUnion
    {
        public int? UnionId { get; set; }
        public string UnionName { get; set; }
        public string RankGroup { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }       
        public DateTime? ModifiedDate { get; set; }     
    }
    public class TblImportCBA
    {
        public int? CBAId { get; set; }
        public string CBAName { get; set; }
        public string CBADescription { get; set; }
        public string Currency { get; set; }
        public string Version { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Attachment { get; set; }
        public bool? IsPF { get; set; }
        public bool? IsGratuity { get; set; }
        public bool? IsAVC { get; set; }
        public bool? IsNUSI { get; set; }
        public int? CBAUnionId { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Boolean IsLocked { get; set; }
    }

    public class TblImportOverTime
    {
        public int? OTId { get; set; }
        public int RankId { get; set; }
        public string OTRate { get; set; }
        public int CBAId { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
    }

    public class TblImportCourseRegister
    {
        public int? CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public string CourseType { get; set; }
        public string Reference { get; set; }
        public string Level { get; set; }
        public string Method { get; set; }
        public string RankId { get; set; }
        public string Group { get; set; }
        public bool? ExpiryApplicable { get; set; }
        public bool? RenewalRequired { get; set; }
        public bool? AuthenticationRequired { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
    public class TblImportState
    {
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public string StateName { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
    public class TblImportcity
    {
        public int CityId { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public string CityName { get; set; }
        public string DomAirport { get; set; }
        public string IntAirport { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
    public class TblImportSeaport
    {
        public int SeaportId { get; set; }
        public int? CountryId { get; set; }
        public string SeaportName { get; set; }
        public string Code { get; set; }
        public bool? IsDeleted { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime RecDate { get; set; }
    }
}
