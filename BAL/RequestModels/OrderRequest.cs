using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.RequestModels
{
    public class OrderRequest
    {
        public string? OrderId { get; set; }
        public string CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public int ShippingMethodId { get; set; }
        public int OrderStatusId { get; set; }
        public string? TrackingNumber { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerProduct { get; set; }
        public string VendorId { get; set; }
  
    }
}
