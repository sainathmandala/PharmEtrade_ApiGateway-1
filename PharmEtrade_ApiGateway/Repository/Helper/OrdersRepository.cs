using BAL.BusinessLogic.Interface;
using BAL.RequestModels;
using BAL.ResponseModels;
using PharmEtrade_ApiGateway.Repository.Interface;
using BAL.Models;
using BAL.BusinessLogic.Helper;

namespace PharmEtrade_ApiGateway.Repository.Helper
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly IOrders _orders;
        public OrdersRepository(IOrders orders)
        {
            _orders = orders;
        }
        public async Task<OrderResponse> AddOrder(TempOrderRequest orderRequest)
        {
            OrderResponse response = await _orders.AddOrder(orderRequest);
            return response;
        }

        public async Task<Response<Order>> GetOrdersByCustomerId(string customerId)
        {
            return await _orders.GetOrdersByCustomerId(customerId);
        }
        

        public async Task<Response<Order>> GetOrdersBySellerId(string vendorId)
        {
            return await _orders.GetOrdersBySellerId(vendorId);
        }
        public async Task<PaymentResponse> AddPayment(PaymentRequest paymentRequest)
        {
            PaymentResponse response = await _orders.AddPayment(paymentRequest);
            return response;
        }

        public async Task<OrderResponse> AddUpdateOrder(OrderRequest orderRequest)
        {
            return await _orders.AddUpdateOrder(orderRequest);
        }

        public  async Task<Response<Order>> GetOrdersByOrderId(string orderid)
        {
            return await _orders.GetOrdersByOrderId(orderid);
        }

        public async Task<Response<Order>> GetCustomersOrderedForSeller(string VendorId)
        {
            return await _orders.GetCustomersOrderedForSeller(VendorId);
        }

    }
}
