using BAL.ResponseModels;

namespace PharmEtrade_ApiGateway.Repository.Interface
{
    public interface IDashboardRepository
    {
        Task<SellerDashboardResponse> GetSellerDashboard(string sellerId);
        Task<BuyerDashboardResponse> GetBuyerDashboard(string buyerId);
        Task<AdminDashboardResponse> GetAdminDashboard(string adminId);
    }
}
