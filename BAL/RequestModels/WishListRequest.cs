using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.RequestModels
{
    public  class WishListRequest
    {
        public string?  WishListId { get; set; }
        public string CustomerId { get; set; }
        public string ProductId { get; set; }
        public int? IsActive { get; set; }
    }
}
