﻿using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblEmail
    {
        public int ID { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public string Smtp { get; set; }
        public string Pop { get; set; }
        public int? Port { get; set; }
        public int? PopPort { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string EmailSentTo { get; set; }
        public string NotificationEmailSentTo { get; set; }
    }
}
