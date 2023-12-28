using System;
using System.Collections.Generic;

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
}
