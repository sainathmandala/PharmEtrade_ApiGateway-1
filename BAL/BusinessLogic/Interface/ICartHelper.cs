using BAL.Models;
using BAL.RequestModels;
using BAL.ResponseModels;

namespace BAL.BusinessLogic.Interface
{
    public interface ICartHelper
    {
        Task<Response<Cart>> GetCartItems(string customerId = null, string productId = null);
        Task<Response<Cart>> AddToCart(CartRequest request);
    }
}
