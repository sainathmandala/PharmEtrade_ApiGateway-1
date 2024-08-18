using BAL.Models;
using BAL.ResponseModels;

namespace BAL.BusinessLogic.Interface
{
    public interface ICartHelper
    {
        Task<Response<Cart>> GetCartItems(string customerId = null, string productId = null);
    }
}
