using BAL.RequestModels;
using BAL.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL.Models;

namespace BAL.BusinessLogic.Interface
{
    public interface IOrders
    {
        Task<OrderResponse> AddOrder(TempOrderRequest orderRequest);
        Task<Response<Order>> GetOrdersByCustomerId(string customerId);
        Task<Response<Order>> GetOrdersBySellerId(string VendorId);
        Task<PaymentResponse> AddPayment(PaymentRequest paymentRequest);
        Task<OrderResponse> AddUpdateOrder(OrderRequest orderRequest);
        Task<Response<Order>> GetOrdersByOrderId(string orderId);
        Task<Response<Order>> GetCustomersOrderedForSeller(string VendorId);
        Task<MemoryStream> DownloadInvoice(string orderId);
        Task<string> DownloadInvoiceHtml(string orderId);
        Task<OrderResponse> SendInvoiceByMail(string orderId);
    }
}
