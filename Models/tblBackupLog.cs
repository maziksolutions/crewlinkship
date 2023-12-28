using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace crewlinkship.Models
{
    public class tblBackupLog
    {
        public int BackupId { get; set; }
        public string LogDescription { get; set; }
        public DateTime RecDate { get; set; }
    }

    public class tblBackupLogModel
    {
        public List<tblBackupLog> tblBackupLog { get; set; }
        public int CurrentPageIndex { get; set; }
        public int PageCount { get; set; }
    }
}
