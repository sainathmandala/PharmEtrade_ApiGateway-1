using BAL.Models;
using BAL.ResponseModels;

namespace PharmEtrade_ApiGateway.Repository.Interface
{
    public interface ICartRepository
    {
        Task<Response<Cart>> GetCartItems(string customerId = null, string productId = null);
    }
}
