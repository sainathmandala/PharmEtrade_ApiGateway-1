using BAL.BusinessLogic.Interface;
using BAL.Models.FedEx;
using BAL.Models.FedEx.RateRequest;
using BAL.Models.FedEx.RateResponse;
using PharmEtrade_ApiGateway.Repository.Interface;

namespace PharmEtrade_ApiGateway.Repository.Helper
{
    public class FedExRepository : IFedExRepository
    {
        private readonly IFedExHelper _fedExHelper;
        public FedExRepository(IFedExHelper fedExHelper)
        {
            this._fedExHelper = fedExHelper;
        }
        public async Task<TokenResponse> GenerateToken()
        {
            return await _fedExHelper.GenerateToken();
        }
        public async Task<RateResponse> GetRates(RateRequest request)
        {
            return await _fedExHelper.GetRates(request);
        }
        public async Task<List<object>> GetServiceTypes(RateRequest request)
        {
            return await _fedExHelper.GetServiceTypes(request);
        }
        public async Task<TrackingResponseModel> GetTrackingInfo(string trackingNumber)
        {
            return await _fedExHelper.GetTrackingInfo(trackingNumber);
        }
    }
}
