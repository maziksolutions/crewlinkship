using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblImportContract
    {
        public int ContractId { get; set; }
        public int? CrewId { get; set; }
        public int? Cbaid { get; set; }
        public int? SeaportId { get; set; }
        public int? VesselId { get; set; }
        public double? Osa { get; set; }
        public double? Waf { get; set; }
        public string ContractPath { get; set; }
        public double? TotalWage { get; set; }
        public string EngagementPort { get; set; }
        public string ReptriationPort { get; set; }
        public double? Sca { get; set; }
        public double? Other { get; set; }
        public double? Seniority { get; set; }
        public string ReviseReason { get; set; }
        public string Duration { get; set; }
        public double? Plus { get; set; }
        public DateTime? SignonDate { get; set; }
        public DateTime? PayCommence { get; set; }
        public DateTime? Expirydate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public double? Pf { get; set; }
        public double? Ud { get; set; }
        public double? Wf { get; set; }
        public double? Pfamount { get; set; }
        public double? Udamount { get; set; }
        public double? Wfamount { get; set; }
        public double? BasicWage { get; set; }
        public double? FixedOvertime { get; set; }
        public double? LeaveWages { get; set; }
        public double? PensionFund { get; set; }
        public double? SubsistenceAllowance { get; set; }
        public double? UniformAllowance { get; set; }
        public bool? Acmapproval { get; set; }
        public int? AcmapprovedBy { get; set; }
        public int? GwapprovedBy { get; set; }
        public bool? Gwapproval { get; set; }
        public bool? IsOnlyBasic { get; set; }
        public string Note { get; set; }
        public int? CrewListId { get; set; }
        public string Gwpath { get; set; }
    }
    //[Table("TblImportBudgetCode")]
    //public class BudgetCode
    //{
    //    public int BudgetCodeId { get; set; }
    //    public string BudgetCodeDescription { get; set; }
    //    public string BudgetCodes { get; set; }
    //    public string ModifiedBy { get; set; }
    //    public DateTime? ModifiedDate { get; set; }
    //    public bool? IsDeleted { get; set; }
    //    public DateTime? RecDate { get; set; }
    //}
    //[Table("TblImportBudgetSubCode")]
    //public class BudgetSubCode
    //{
    //    public int SubCodeId { get; set; }
    //    public int BudgetCodeId { get; set; }
    //    public int SubCode { get; set; }
    //    public string SubBudget { get; set; }
    //    public string Description { get; set; }
    //    public string ModifiedBy { get; set; }
    //    public DateTime? ModifiedDate { get; set; }
    //    public bool? IsDeleted { get; set; }
    //    public DateTime? RecDate { get; set; }
    //}
    //[Table("TblImportWageComponent")]   
    //public class WageComponent
    //{
    //    public int WageId { get; set; }
    //    public int? BudgetCodeId { get; set; }
    //    public int? SubCodeId { get; set; }
    //    public string CalculationBasis { get; set; }
    //    public string PayableBasis { get; set; }
    //    public string IncludedOnboard { get; set; }
    //    public string Earning { get; set; }
    //    public string IsCBA { get; set; }
    //    public bool? IsShowAll { get; set; }
    //    public bool? IsDeleted { get; set; }
    //    public DateTime? RecDate { get; set; }
    //    public string ModifiedBy { get; set; }
    //    public DateTime? ModifiedDate { get; set; }
    //    public int? ColumnConfigId { get; set; }     
    //}
    //[Table("TblImportWageStructure")]
    //public class WageStructure
    //{
    //    public int WageStructureId { get; set; }
    //    public int RankId { get; set; }
    //    public int? WageId { get; set; }
    //    public string WageAmount { get; set; }
    //    public int CBAId { get; set; }
    //    public string ModifiedBy { get; set; }
    //    public DateTime? ModifiedDate { get; set; }
    //    public bool? IsDeleted { get; set; }
    //    public DateTime? RecDate { get; set; }
    //}
    //[Table("TblImportCBA")]
    //public class CBA
    //{
    //    public int CBAId { get; set; }
    //    public string CBAName { get; set; }
    //    public string CBADescription { get; set; }
    //    public string Currency { get; set; }
    //    public string Version { get; set; }
    //    public DateTime StartDate { get; set; }
    //    public DateTime EndDate { get; set; }
    //    public string Attachment { get; set; }
    //    public bool? IsPF { get; set; }
    //    public bool? IsGratuity { get; set; }
    //    public bool? IsAVC { get; set; }
    //    public bool? IsNUSI { get; set; }
    //    public int? CBAUnionId { get; set; }
    //    public bool? IsDeleted { get; set; }
    //    public DateTime? RecDate { get; set; }
    //    public string ModifiedBy { get; set; }
    //    public DateTime? ModifiedDate { get; set; }
    //    public Boolean IsLocked { get; set; }
    //}
    //[Table("TblImportOverTime")]
    //public class OverTime
    //{
    //    public int OTId { get; set; }
    //    public int RankId { get; set; }
    //    public string OTRate { get; set; }
    //    public int CBAId { get; set; }
    //    public string ModifiedBy { get; set; }
    //    public DateTime? ModifiedDate { get; set; }
    //    public bool? IsDeleted { get; set; }
    //    public DateTime? RecDate { get; set; }
    //}
    //[Table("TblImportCourseRegister")]
    //public class CourseRegister
    //{
    //    public int CourseId { get; set; }
    //    public string CourseName { get; set; }
    //    public string CourseCode { get; set; }
    //    public string CourseType { get; set; }
    //    public string Reference { get; set; }
    //    public string Level { get; set; }
    //    public string Method { get; set; }
    //    public string RankId { get; set; }
    //    public string Group { get; set; }
    //    public bool? ExpiryApplicable { get; set; }
    //    public bool? RenewalRequired { get; set; }
    //    public bool? AuthenticationRequired { get; set; }
    //    public bool? IsDeleted { get; set; }
    //    public DateTime? RecDate { get; set; }
    //    public string ModifiedBy { get; set; }
    //    public DateTime? ModifiedDate { get; set; }
    //}
    //[Table("TblImportState")]
    //public  class State
    //{
    //    public int StateId { get; set; }
    //    public int CountryId { get; set; }
    //    public string StateName { get; set; }
    //    public bool? IsDeleted { get; set; }
    //    public DateTime? RecDate { get; set; }
    //    public string ModifiedBy { get; set; }
    //    public DateTime? ModifiedDate { get; set; }
    //}
    //[Table("TblImportcity")]
    //public class City
    //{
    //    public int CityId { get; set; }
    //    public int? CountryId { get; set; }
    //    public int? StateId { get; set; }
    //    public string CityName { get; set; }
    //    public string DomAirport { get; set; }
    //    public string IntAirport { get; set; }
    //    public bool? IsDeleted { get; set; }
    //    public DateTime? RecDate { get; set; }
    //    public string ModifiedBy { get; set; }
    //    public DateTime? ModifiedDate { get; set; }
    //}
    //[Table("TblImportSeaport")]    
    //public class Seaport
    //{
    //    public int SeaportId { get; set; }
    //    public int? CountryId { get; set; }
    //    public string SeaportName { get; set; }
    //    public string Code { get; set; }
    //    public bool? IsDeleted { get; set; }
    //    public string ModifiedBy { get; set; }
    //    public DateTime RecDate { get; set; }
    //}
}
