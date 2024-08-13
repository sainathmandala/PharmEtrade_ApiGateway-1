using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ResponseModels
{
    public class OrderResponse
    {
        public string OrderId { get; set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public string VendorName { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
    }
}
