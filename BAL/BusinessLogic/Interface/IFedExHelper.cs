using BAL.Models.FedEx;
using BAL.Models.FedEx.RateRequest;

namespace BAL.BusinessLogic.Interface
{
    public interface IFedExHelper
    {
        Task<TokenResponse> GenerateToken();
        Task<TrackingResponseModel> GetTrackingInfo(string trackingNumber);
        Task<HttpResponseMessage> GetRates(RateRequest request);
    }
}
