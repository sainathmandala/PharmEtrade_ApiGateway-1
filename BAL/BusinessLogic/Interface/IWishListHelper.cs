using BAL.Models;
using BAL.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL.ResponseModels;

namespace BAL.BusinessLogic.Interface
{
    public  interface IWishListHelper
    {
        Task<Response<WishList>> GetWishListItems(string customerId = null);
        Task<Response<WishList>> AddToWishList(WishListRequest request);
        Task<Response<WishList>> GetWishListById(string WishListId = null);
    }
}
