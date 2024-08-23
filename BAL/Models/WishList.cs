using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public  class WishList
    {
        public string? WishListId { get; set; }
        public CustomerBasicDetails Customer { get; set; }
        public ProductBasicDetails Product { get; set; }
        public bool IsActive { get; set; }
        public DateTime DeletedOn { get; set; }

    }
}
