using BAL.BusinessLogic.Interface;
using BAL.RequestModels;
using BAL.ResponseModels;
using PharmEtrade_ApiGateway.Repository.Interface;

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
    }
}
