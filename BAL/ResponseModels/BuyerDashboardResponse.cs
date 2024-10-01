using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ResponseModels
{
    public class BuyerDashboardResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public int TotalOrders { get; set; }
        public int UpcomingOrders { get; set; }
        public int ReceivedOrders { get; set; }
        public int CancelledOrders { get; set; }
        public int ProductsOrdered { get; set; }
        public decimal TotalPurchaseValue { get; set; }
        public int WishList { get; set; }
    }
}
