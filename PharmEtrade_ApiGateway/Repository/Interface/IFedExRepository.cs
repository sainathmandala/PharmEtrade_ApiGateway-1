using BAL.Models.FedEx;

namespace PharmEtrade_ApiGateway.Repository.Interface
{
    public interface IFedExRepository
    {
        Task<TokenResponse> GenerateToken();
        Task<TrackingResponseModel> GetTrackingInfo(string trackingNumber);
    }
}
