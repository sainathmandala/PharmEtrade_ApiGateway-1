using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class RoleMaster
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
