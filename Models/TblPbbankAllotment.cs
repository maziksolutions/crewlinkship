using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblPbbankAllotment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BankAllotmentId { get; set; }
        public int Crew { get; set; }
        public int VesselId { get; set; }
        public int BankId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public double? Allotments { get; set; }
        public bool IsMidMonthAllotment { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Recdate { get; set; }
        public bool? IsPromoted { get; set; }
    }

    public class tblPBBankAllotmentVM
    {
        public int VesselPortId { get; set; }
        public int Crew { get; set; }
        public int VesselId { get; set; }
        public int BankId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public double? Allotments { get; set; }
        public bool IsMidMonthAllotment { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Recdate { get; set; }
        public bool? IsPromoted { get; set; }
    }
}
