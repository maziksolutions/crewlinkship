using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace crewlinkship.Models
{
    public partial class tblimportPortageEarningDedu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PortageEarningDeductionId { get; set; }
        public int? VesselPortId { get; set; }
        public int? OfficePBId { get; set; }
        public int? CrewId { get; set; }
        public int? PortageBillId { get; set; }
        public int? Vesselid { get; set; }
        public int? SubCodeId { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string Type { get; set; }
        public string Currency { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsPortagelocked { get; set; }
    }
}
