using BAL.BusinessLogic.Interface;
using BAL.ViewModel;
using BAL.ViewModels;
using PharmEtrade_ApiGateway.Repository.Interface;
using System.Threading.Tasks;
using BAL.Common;
using System.IO;
using BAL.Models;
using BAL.BusinessLogic.Helper;
using BAL.ResponseModels;
using BAL.RequestModels;



namespace PharmEtrade_ApiGateway.Repository.Helper
{
    public class BidRepository : IBidRepository
    {

        private readonly IBidHealper _BidHelper;

        public BidRepository(IBidHealper BidHelper)
        {
            _BidHelper = BidHelper;
        }

        public async Task<Response<BidResponse>> AddBid(Bid bid)
        {
            return await _BidHelper.AddBid(bid);
        }

        public async Task<Response<BidResponse>> UpdateBid(Bid bid)
        {
            return await _BidHelper.UpdateBid(bid);

        }

        public async Task<Response<BidResponse>> GetBidsByBuyer(string BidId)
        {
            return await _BidHelper.GetBidsByBuyer(BidId);
        }

        public async Task<Response<BidResponse>> GetBidsBySeller(string BidId)
        {
            return await _BidHelper.GetBidsByBuyer(BidId);
        }


        public async Task<Response<BidResponse>> GetBidsByProduct(string productId)
        {
            return await _BidHelper.GetBidsByProduct(productId);
        }

        public async Task<Response<BidResponse>> GetProductsQuotesByBuyer(string BuyerId)
        {
            return await _BidHelper.GetProductsQuotesByBuyer(BuyerId);
        }

        public async Task<Response<BidResponse>> GetProductsQuotesBySeller(string sellerId)
        {
            return await _BidHelper.GetProductsQuotesBySeller(sellerId);
        }

        public async  Task<Response<string>> RemoveBid(string bidId)
        {
            return await _BidHelper.RemoveBid(bidId);
        }
    }
}
