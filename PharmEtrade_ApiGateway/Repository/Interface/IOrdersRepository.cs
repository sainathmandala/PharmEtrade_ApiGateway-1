using BAL.Models;
using BAL.RequestModels;
using BAL.ResponseModels;
namespace PharmEtrade_ApiGateway.Repository.Interface
{
    public interface IOrdersRepository
    {
        Task<OrderResponse> AddOrder(TempOrderRequest orderRequest);
        Task<Response<Order>> GetOrdersByCustomerId(string customerId);
        Task<Response<Order>> GetOrdersBySellerId(string VendorId);
        Task<PaymentResponse> AddPayment(PaymentRequest paymentRequest);
        //Task<Response<SpecialOffersResponse>> GetSpecialOffers();
        Task<OrderResponse> AddUpdateOrder(OrderRequest orderRequest);
        Task<Response<Order>> GetOrdersByOrderId(string OrderId);
        Task<MemoryStream> DownloadInvoice(string orderId);
        Task<OrderResponse> SendInvoiceByMail(string orderId);
        Task<Response<Order>> GetCustomersOrderedForSeller(string VendorId);
        Task<string> DownloadInvoiceHtml(string orderId);
    }
}
