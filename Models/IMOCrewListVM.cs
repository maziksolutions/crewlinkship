using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace crewlinkship.Models
{
    public class IMOCrewListVM
    {
        public int CrewListId { get; set; }
        public int RowNumber { get; set; }
        public int Ranksort { get; set; }
        public string Rank { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string nationality { get; set; }
        public DateTime DOB { get; set; }
        public string BirthPlace { get; set; }
        public string PassportNo { get; set; }
    }
}
