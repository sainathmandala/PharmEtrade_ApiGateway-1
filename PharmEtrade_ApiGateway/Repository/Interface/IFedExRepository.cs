using BAL.Models.FedEx;
using BAL.Models.FedEx.RateRequest;

namespace PharmEtrade_ApiGateway.Repository.Interface
{
    public interface IFedExRepository
    {
        Task<TokenResponse> GenerateToken();
        Task<TrackingResponseModel> GetTrackingInfo(string trackingNumber);
        Task<HttpResponseMessage> GetRates(RateRequest request);
    }
}
