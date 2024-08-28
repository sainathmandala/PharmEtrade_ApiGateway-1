using BAL.Models;
using BAL.RequestModels;
using BAL.ResponseModels;

namespace PharmEtrade_ApiGateway.Repository.Interface
{
    public interface ICartRepository
    {
        Task<Response<Cart>> GetCartItems(string customerId = null, string productId = null);
        Task<Response<Cart>> AddToCart(CartRequest request);
        Task<Response<Cart>> DeleteCart(string CartId);
    }
}
