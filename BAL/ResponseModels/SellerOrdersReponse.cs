using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ResponseModels
{
    public class SellerOrdersReponse
    {
        public string OrderId { get; set; }
        public string? CustomerId { get; set; }

        public string? ProductId { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address1 { get; set; }

        public string ProductName { get; set; }

        public double TotalAmount { get; set; }

        public int ShippingMethodId { get; set; }

        public int OrderStatusId { get; set; }

        public string TrackingNumber { get; set; }

        public string OrderDetailId { get; set; }
        public int Quantity { get; set; }
        public double PricePerProduct { get; set; }
        public string VendorId { get; set; }

        public string ProductDescription { get; set; }
        public DateTime OrderDate { get; set; }

        public string? ImageUrl { get; set; }
        public string Pincode { get; set; }
        public string Address2 { get; set; }    
        public string Landmark { get; set; }    
        public string City { get; set; }    
        public string State { get; set; }   
        public string Country { get; set; } 
    }
}
