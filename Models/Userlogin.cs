using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace crewlinkship.Models
{
    public partial class Userlogin
    {
        [Key]
        public int UerId { get; set; }
        public string UserName { get; set; }
        public string UserType { get; set; }
        public string Password { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
    }
}
