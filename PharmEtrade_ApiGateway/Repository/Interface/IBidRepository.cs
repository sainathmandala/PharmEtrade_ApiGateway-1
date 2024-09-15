using BAL.Models;
using BAL.ResponseModels;

namespace PharmEtrade_ApiGateway.Repository.Interface
{
    public interface IBidRepository
    {

        Task<BAL.ResponseModels.Response<BidResponse>> AddBid(Bid bid);
        Task<BAL.ResponseModels.Response<BidResponse>> UpdateBid(Bid bid);
        Task<Response<string>> RemoveBid(string bidId);
        Task<BAL.ResponseModels.Response<BidResponse>> GetBidsByBuyer(string BidId = null);
        Task<BAL.ResponseModels.Response<BidResponse>> GetBidsBySeller(string sellerId = null);
        Task<BAL.ResponseModels.Response<BidResponse>> GetBidsByProduct(string productId);
        Task<BAL.ResponseModels.Response<BidResponse>> GetProductsQuotesByBuyer(string BuyerId);
        Task<BAL.ResponseModels.Response<BidResponse>> GetProductsQuotesBySeller(string sellerId);

    }
}
