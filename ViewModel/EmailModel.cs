using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace crewlinkship.ViewModel
{
    public class EmailModel
    {
        public int MessageNumber { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime DateSent { get; set; }
    }
}
