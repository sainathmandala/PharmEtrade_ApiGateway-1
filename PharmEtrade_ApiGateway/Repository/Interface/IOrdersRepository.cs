using BAL.RequestModels;
using BAL.ResponseModels;
using BAL.ViewModels;
namespace PharmEtrade_ApiGateway.Repository.Interface
{
    public interface IOrdersRepository
    {
        Task<OrderResponse> AddOrder(OrderRequest orderRequest);
        Task<List<Order>> GetOrdersByCustomerId(string customerId);
    }
}
