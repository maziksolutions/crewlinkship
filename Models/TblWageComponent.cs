using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblWageComponent
    {
        public TblWageComponent()
        {
            TblWageStructures = new HashSet<TblWageStructure>();
        }

        public int WageId { get; set; }
        public int? BudgetCodeId { get; set; }
        public int? SubCodeId { get; set; }
        public string CalculationBasis { get; set; }
        public string PayableBasis { get; set; }
        public string IncludedOnboard { get; set; }
        public string Earning { get; set; }
        public string IsCba { get; set; }
        public bool? IsShowAll { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblBudgetCode BudgetCode { get; set; }
        public virtual TblBudgetSubCode SubCode { get; set; }
        public virtual ICollection<TblWageStructure> TblWageStructures { get; set; }
    }
}
