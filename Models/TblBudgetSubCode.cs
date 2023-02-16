using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblBudgetSubCode
    {
        public TblBudgetSubCode()
        {
            TblWageComponents = new HashSet<TblWageComponent>();
        }

        public int SubCodeId { get; set; }
        public int BudgetCodeId { get; set; }
        public int SubCode { get; set; }
        public string SubBudget { get; set; }
        public string Description { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }

        public virtual TblBudgetCode BudgetCode { get; set; }
        public virtual ICollection<TblWageComponent> TblWageComponents { get; set; }
    }
}
