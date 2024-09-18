using BAL.Models;
using BAL.ResponseModels;

namespace PharmEtrade_ApiGateway.Repository.Interface
{
    public interface IBidRepository
    {

        Task<BAL.ResponseModels.Response<BidsResponse>> AddBid(Bids bid);
        Task<BAL.ResponseModels.Response<BidsResponse>> UpdateBid(Bids bid);
        Task<Response<string>> RemoveBid(string bidId);
        Task<BAL.ResponseModels.Response<BidsResponse>> GetBidsByBuyer(string BidId = null);
        Task<BAL.ResponseModels.Response<BidsResponse>> GetBidsBySeller(string sellerId = null);
        Task<BAL.ResponseModels.Response<BidsResponse>> GetBidsByProduct(string productId);
        Task<BAL.ResponseModels.Response<BidsResponse>> GetProductsQuotesByBuyer(string BuyerId);
        Task<BAL.ResponseModels.Response<BidsResponse>> GetProductsQuotesBySeller(string sellerId);

    }
}
