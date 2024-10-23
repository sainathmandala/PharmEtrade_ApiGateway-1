using BAL.Models.FedEx;
using BAL.Models.FedEx.RateRequest;
using BAL.Models.FedEx.RateResponse;

namespace PharmEtrade_ApiGateway.Repository.Interface
{
    public interface IFedExRepository
    {
        Task<TokenResponse> GenerateToken();
        Task<TrackingResponseModel> GetTrackingInfo(string trackingNumber);
        Task<RateResponse> GetRates(RateRequest request);
    }
}
