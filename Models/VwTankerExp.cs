using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class VwTankerExp
    {
        public string TankerExperience { get; set; }
        public string TotalExperience { get; set; }
        public int? Crewid { get; set; }
        public int? VesselTypeid { get; set; }
    }
}
