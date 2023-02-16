using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblBudgetCode
    {
        public TblBudgetCode()
        {
            TblBudgetSubCodes = new HashSet<TblBudgetSubCode>();
            TblWageComponents = new HashSet<TblWageComponent>();
        }

        public int BudgetCodeId { get; set; }
        public string BudgetCodeDescription { get; set; }
        public string BudgetCodes { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }

        public virtual ICollection<TblBudgetSubCode> TblBudgetSubCodes { get; set; }
        public virtual ICollection<TblWageComponent> TblWageComponents { get; set; }
    }
}
