using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class Menu
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public string? NavigateUrl { get; set; }
        public string? Title { get; set; }
        public string? IconPath { get; set; }
        public int Parent { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int AccountTypeId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string CustomerTypeId { get; set; }
    }
}
