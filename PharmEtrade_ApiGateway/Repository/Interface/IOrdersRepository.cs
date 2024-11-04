using BAL.Models;
using BAL.RequestModels;
using BAL.ResponseModels;
namespace PharmEtrade_ApiGateway.Repository.Interface
{
    public interface IOrdersRepository
    {
        Task<OrderResponse> AddOrder(TempOrderRequest orderRequest);
        Task<Response<Order>> GetOrdersByCustomerId(string customerId);
        Task<Response<Order>> GetCustomerOrdersByDate(BuyerOrderCriteria orderCriteria);
        Task<Response<SellerOrdersReponse>> GetOrdersBySellerId(string VendorId);
        Task<Response<Order>> GetSellerOrdersByDate(SellerOrderCriteria orderCriteria);
        Task<PaymentResponse> AddPayment(PaymentRequest paymentRequest);
        //Task<Response<SpecialOffersResponse>> GetSpecialOffers();
        Task<OrderResponse> AddUpdateOrder(OrderRequest orderRequest);
        Task<OrderResponse> UpdateDeliveryAddress(string customerId, string orderId, string addressId);
        Task<OrderResponse> UpdateOrderStatus(string orderId, int statusId);
        Task<Response<Order>> GetOrdersByOrderId(string OrderId);
        Task<Response<Order>> GetOrdersByDate(OrderCriteria orderCriteria);
        Task<MemoryStream> DownloadInvoice(string orderId);
        Task<OrderResponse> SendInvoiceByMail(string orderId);
        Task<Response<Order>> GetCustomersOrderedForSeller(string VendorId);
        Task<string> DownloadInvoiceHtml(string orderId);
        Task<Response<Shipments>> AddUpdateShipmentDetail(Shipments shipments);
        Task<Response<Shipments>> GetShipmentsByCustomerId(string customerId);
        Task<Response<BAL.Models.SquareupPayments.SquareupPaymentResponse>> ProcessPaymentRequest(BAL.Models.SquareupPayments.SquareupPaymentRequest request);
    }
}
