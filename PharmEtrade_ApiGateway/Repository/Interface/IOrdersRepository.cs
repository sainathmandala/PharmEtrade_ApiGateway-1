using BAL.RequestModels;
using BAL.ResponseModels;

namespace PharmEtrade_ApiGateway.Repository.Interface
{
    public interface IOrdersRepository
    {
        Task<OrderResponse> AddOrder(OrderRequest orderRequest);
        Task<List<OrderResponse>> GetOrdersByCustomerId(string customerId);
    }
}
