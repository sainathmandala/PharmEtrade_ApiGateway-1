using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ResponseModels
{
    public class SellerDashboardResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public int TotalOrders { get; set; }
        public int ProductsOrdered { get; set; }
        public int CustomersOrdered { get; set; }
        public decimal TotalSaleValue { get; set; }
        public int TotalProducts { get; set; }
        public int ActiveProducts { get; set; }
    }
}
