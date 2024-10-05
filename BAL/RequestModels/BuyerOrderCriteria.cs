using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.RequestModels
{
    public class BuyerOrderCriteria
    {
        public string? BuyerId { get; set; }
        public DateTime? OrderFromDate { get; set; }
        public DateTime? OrderToDate { get; set; }

    }

    public class SellerOrderCriteria
    {
        public string? SellerId { get; set; }
        public DateTime? OrderFromDate { get; set; }
        public DateTime? OrderToDate { get; set; }
    }

    public class OrderCriteria
    {
        public DateTime? OrderFromDate { get; set; }
        public DateTime? OrderToDate { get; set; }
    }

    public class PaymentCriteria
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

}
