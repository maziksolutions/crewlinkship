using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblIssuingAuthority
    {
        public int AuthorityId { get; set; }
        public int CountryId { get; set; }
        public string IssuingAuthorities { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }

        public virtual TblCountry Country { get; set; }
    }
}
