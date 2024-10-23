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
        Task<OrderResponse> UpdateDeliveryAddress(string customerId, string orderId, string addressId);
        Task<Response<Order>> GetOrdersByCustomerId(string customerId);
        Task<Response<Order>> GetCustomerOrdersByDate(BuyerOrderCriteria orderCriteria);
        Task<Response<Order>> GetOrdersBySellerId(string VendorId);
        Task<Response<Order>> GetSellerOrdersByDate(SellerOrderCriteria orderCriteria);
        Task<PaymentResponse> AddPayment(PaymentRequest paymentRequest);
        Task<OrderResponse> AddUpdateOrder(OrderRequest orderRequest);
        Task<Response<Order>> GetOrdersByOrderId(string orderId);
        Task<Response<Order>> GetOrdersByDate(OrderCriteria orderCriteria);
        Task<Response<Order>> GetCustomersOrderedForSeller(string VendorId);
        Task<MemoryStream> DownloadInvoice(string orderId);
        Task<string> DownloadInvoiceHtml(string orderId);
        Task<OrderResponse> SendInvoiceByMail(string orderId);

        Task<Response<Shipments>> AddUpdateShipmentDetail(Shipments shipments);
        Task<Response<Shipments>> GetShipmentsByCustomerId(string customerId);

    }
}
