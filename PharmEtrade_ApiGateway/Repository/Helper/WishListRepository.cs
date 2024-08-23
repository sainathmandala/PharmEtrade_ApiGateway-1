using BAL.BusinessLogic.Interface;
using BAL.Models;
using BAL.RequestModels;
using PharmEtrade_ApiGateway.Repository.Interface;
using BAL.ResponseModels;

namespace PharmEtrade_ApiGateway.Repository.Helper
{
    public class WishListRepository : IWishListRepository
    {
        
            private IWishListHelper _wishListHelper;

            public WishListRepository(IWishListHelper wishListHelper)
            {
               _wishListHelper = wishListHelper;
            }

            public async Task<Response<WishList>> AddToWishList(WishListRequest request)
            {
                return await _wishListHelper.AddToWishList(request);
            }

            public async Task<Response<WishList>> GetWishListItems(string customerId = null)
            {
                return await _wishListHelper.GetWishListItems(customerId);
            }
        
    }
}
