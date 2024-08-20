using BAL.BusinessLogic.Interface;
using BAL.Models;
using BAL.RequestModels;
using BAL.ResponseModels;
using PharmEtrade_ApiGateway.Repository.Interface;

namespace PharmEtrade_ApiGateway.Repository.Helper
{
    public class CartRepository : ICartRepository
    {
        private ICartHelper _cartHelper;
        public CartRepository(ICartHelper cartHelper) {
            _cartHelper = cartHelper;
        }

        public async Task<Response<Cart>> AddToCart(CartRequest request)
        {
            return await _cartHelper.AddToCart(request);
        }

        public async Task<Response<Cart>> GetCartItems(string customerId = null, string productId = null)
        {
            return await _cartHelper.GetCartItems(customerId, productId);
        }
    }
}
