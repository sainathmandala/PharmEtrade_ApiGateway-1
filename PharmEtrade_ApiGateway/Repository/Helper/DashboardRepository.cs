using BAL.BusinessLogic.Interface;
using BAL.ResponseModels;
using PharmEtrade_ApiGateway.Repository.Interface;

namespace PharmEtrade_ApiGateway.Repository.Helper
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly IDashboardHelper _dashboardHelper;
        public DashboardRepository(IDashboardHelper dashboardHelper) { 
            _dashboardHelper = dashboardHelper;
        }
        public async Task<AdminDashboardResponse> GetAdminDashboard(string adminId)
        {
            return await _dashboardHelper.GetAdminDashboard(adminId);
        }

        public async Task<BuyerDashboardResponse> GetBuyerDashboard(string buyerId)
        {
            return await _dashboardHelper.GetBuyerDashboard(buyerId);
        }

        public async Task<SellerDashboardResponse> GetSellerDashboard(string sellerId)
        {
            return await _dashboardHelper.GetSellerDashboard(sellerId);
        }
    }
}
