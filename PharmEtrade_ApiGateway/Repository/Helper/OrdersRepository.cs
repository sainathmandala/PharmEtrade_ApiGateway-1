using BAL.BusinessLogic.Interface;
using BAL.RequestModels;
using BAL.ResponseModels;
using PharmEtrade_ApiGateway.Repository.Interface;
using BAL.Models;

namespace PharmEtrade_ApiGateway.Repository.Helper
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly IOrders _orders;
        public OrdersRepository(IOrders orders)
        {
            _orders = orders;
        }
        public async Task<OrderResponse> AddOrder(OrderRequest orderRequest)
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
        public async Task<PaymentResponse>AddPayment(PaymentRequest paymentRequest)
        {
            PaymentResponse response = await _orders.AddPayment(paymentRequest);
            return response;
        }
    }
}
