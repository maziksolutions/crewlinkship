using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblBuilder
    {
        public TblBuilder()
        {
            TblVessels = new HashSet<TblVessel>();
        }

        public int BuilderId { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public string Builder { get; set; }
        public string Street { get; set; }
        public string Area { get; set; }
        public string PostCode { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblCity City { get; set; }
        public virtual TblCountry Country { get; set; }
        public virtual TblState State { get; set; }
        public virtual ICollection<TblVessel> TblVessels { get; set; }
    }
}
