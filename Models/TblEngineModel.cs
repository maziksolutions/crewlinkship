using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblEngineModel
    {
        public TblEngineModel()
        {
            TblAssignmentsWithOthers = new HashSet<TblAssignmentsWithOther>();
            TblAssignmentsWithOurs = new HashSet<TblAssignmentsWithOur>();
            TblVessels = new HashSet<TblVessel>();
        }

        public int EngineModelId { get; set; }
        public int? EngineTypeId { get; set; }
        public int? EngineSubTypeId { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public string Model { get; set; }
        public string Maker { get; set; }
        public string Street { get; set; }
        public string Area { get; set; }
        public string PostCode { get; set; }
        public string CountryCode { get; set; }
        public string StateCode { get; set; }
        public string LandlineNo { get; set; }
        public string MobileCode { get; set; }
        public string MobileNo { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblCity City { get; set; }
        public virtual TblCountry Country { get; set; }
        public virtual TblEngineSubType EngineSubType { get; set; }
        public virtual TblEnginetype EngineType { get; set; }
        public virtual TblState State { get; set; }
        public virtual ICollection<TblAssignmentsWithOther> TblAssignmentsWithOthers { get; set; }
        public virtual ICollection<TblAssignmentsWithOur> TblAssignmentsWithOurs { get; set; }
        public virtual ICollection<TblVessel> TblVessels { get; set; }
    }
}
