using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblCba
    {
        public TblCba()
        {
            TblMidMonthAllotments = new HashSet<TblMidMonthAllotment>();
            TblNigerianDeductions = new HashSet<TblNigerianDeduction>();
            TblOverTimes = new HashSet<TblOverTime>();
            TblPfrates = new HashSet<TblPfrate>();
            TblPortageBills = new HashSet<TblPortageBill>();
            TblWageStructures = new HashSet<TblWageStructure>();
        }

        public int Cbaid { get; set; }
        public string Cbaname { get; set; }
        public string Cbadescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Attachment { get; set; }
        public string Version { get; set; }
        public int? CbaunionId { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Currency { get; set; }
        public bool? IsLocked { get; set; }
        public bool? IsAvc { get; set; }
        public bool? IsGratuity { get; set; }
        public bool? IsNusi { get; set; }
        public bool? IsPf { get; set; }

        public virtual TblCbaUnion Cbaunion { get; set; }
        public virtual ICollection<TblMidMonthAllotment> TblMidMonthAllotments { get; set; }
        public virtual ICollection<TblNigerianDeduction> TblNigerianDeductions { get; set; }
        public virtual ICollection<TblOverTime> TblOverTimes { get; set; }
        public virtual ICollection<TblPfrate> TblPfrates { get; set; }
        public virtual ICollection<TblPortageBill> TblPortageBills { get; set; }
        public virtual ICollection<TblWageStructure> TblWageStructures { get; set; }
    }
}
