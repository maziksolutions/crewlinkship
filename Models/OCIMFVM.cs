using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace crewlinkship.Models
{
    public class OCIMFVM
    {
        public long OCIMFVMId { get; set; }
        public string VesselName { get; set; }
        public string RankName { get; set; }
        public string FamilyName { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string Identification { get; set; }
        public string Nationality { get; set; }
        public string CertofComp { get; set; }
        public string IssuingCountry { get; set; }
        public string AdminAccept { get; set; }
        public string TankerCert { get; set; }
        public string SpecializedTankerTraining { get; set; }
        public string RadioQual { get; set; }
        public string EnglishProf { get; set; }
        public string SignOnDate { get; set; }
        public string OperatorExp { get; set; }
        public string RankExperience { get; set; }
        public string TankerExp { get; set; }
        public string AllTankerExp { get; set; }
        public string TotalExperience { get; set; }
    }
}
