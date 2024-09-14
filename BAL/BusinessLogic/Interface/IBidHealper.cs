using BAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL.ResponseModels;
using BAL.RequestModels;


namespace BAL.BusinessLogic.Interface
{
    public  interface IBidHealper
    {

        Task<BAL.ResponseModels.Response<BidResponse>> AddBid(Bid bid);
        Task<BAL.ResponseModels.Response<BidResponse>> UpdateBid(Bid bid);
        Task<Response<string>> RemoveBid(string bidId);
        Task<BAL.ResponseModels.Response<BidResponse>> GetBidsByBuyer(string BidId = null);
        Task<BAL.ResponseModels.Response<BidResponse>> GetBidsBySeller(string sellerId= null);
        Task<BAL.ResponseModels.Response<BidResponse>> GetBidsByProduct(string productId);
        Task<BAL.ResponseModels.Response<BidResponse>> GetProductsQuotesByBuyer(string BuyerId);
        Task<BAL.ResponseModels.Response<BidResponse>> GetProductsQuotesBySeller(string sellerId);



    }
}
