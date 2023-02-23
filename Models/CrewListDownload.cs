using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace crewlinkship.Models
{
    public class CrewListDownload
    {
        
        public int SNo { get; set; }
        public string VesselName { get; set; }
        public string CrewName { get; set; }
        public string Rank { get; set; }
        public string Nationality { get; set; }
        public string Passport { get; set; }
        public string CDC { get; set; }
        public DateTime? DateSignedOn { get; set; }
        public DateTime? ReliefDate { get; set; }
        public string PortOfJoining { get; set; }
        public DateTime? DOB { get; set; }
    }
}
