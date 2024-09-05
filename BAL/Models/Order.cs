namespace BAL.Models
{
    public class Order
    {
        public string OrderId { get; set; }
        public string? CustomerId { get; set; }

        public string? ProductId { get; set; }
        public string CustomerName { get; set; }

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
    }
}
