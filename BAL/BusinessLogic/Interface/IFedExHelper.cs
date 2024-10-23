using BAL.Models.FedEx;
using BAL.Models.FedEx.RateRequest;
using BAL.Models.FedEx.RateResponse;

namespace BAL.BusinessLogic.Interface
{
    public interface IFedExHelper
    {
        Task<TokenResponse> GenerateToken();
        Task<TrackingResponseModel> GetTrackingInfo(string trackingNumber);
        Task<RateResponse> GetRates(RateRequest request);
    }
}
