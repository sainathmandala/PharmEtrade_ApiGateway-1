using BAL.Models;
using BAL.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.BusinessLogic.Interface
{
    public interface IDashboardHelper
    {
        Task<SellerDashboardResponse> GetSellerDashboard(string sellerId);
        Task<BuyerDashboardResponse> GetBuyerDashboard(string buyerId);
        Task<AdminDashboardResponse> GetAdminDashboard(string adminId);
    }
}
