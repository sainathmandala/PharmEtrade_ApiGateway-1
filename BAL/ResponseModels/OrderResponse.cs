using BAL.Models;

namespace BAL.ResponseModels
{
    public class OrderProductResponse
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerProduct { get; set; }
        public string SellerId { get; set; }       
        public string ProductName { get; set; }
        public string SellerName { get; set; }
        public string? ImageUrl { get; set; }
        public decimal ShippingCost { get; set; }
        public string NDCorUPC { get; set; }
        public string SKU { get; set; }
        public string PackQuantity { get; set; }

    }
    public class OrderResponse
    {
        public string OrderId { get; set; }
        public string? OrderNumber { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShippingMethod { get; set; }
        public string ShippingMethodName { get; set; }
        public string OrderStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public string? InvoiceNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderProductResponse> Products { get; set; }
        public CustomerAddress CustomerAddress { get; set; } = new CustomerAddress();
    }
}
