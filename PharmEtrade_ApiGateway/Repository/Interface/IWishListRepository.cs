using BAL.ResponseModels;
using BAL.Models;
using BAL.RequestModels;

namespace PharmEtrade_ApiGateway.Repository.Interface
{
    public interface IWishListRepository
    {
        Task<Response<WishList>> AddToWishList(WishListRequest request);
        Task<Response<WishList>> GetWishListItems(string customerId = null);
        Task<Response<WishList>> GetWishListById(string WishListId = null);
        Task<Response<WishList>> RemoveWishList(string wishlistId);
    }
}
