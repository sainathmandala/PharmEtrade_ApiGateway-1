using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmEtrade_ApiGateway.Repository.Interface;

namespace PharmEtrade_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardRepository _dashboardRepository;
        public DashboardController(IDashboardRepository dashboardRepository) { 
            _dashboardRepository = dashboardRepository;
        }

        [HttpGet("GetAdminDashboard")]
        public async Task<IActionResult> GetAdminDashboard(string? adminId)
        {
            var response = await _dashboardRepository.GetAdminDashboard(string.IsNullOrEmpty(adminId) ? "" : adminId);
            return Ok(response);
        }

        [HttpGet("GetSellerDashboard")]
        public async Task<IActionResult> GetSellerDashboard(string sellerId)
        {
            var response = await _dashboardRepository.GetSellerDashboard(sellerId);
            return Ok(response);
        }

        [HttpGet("GetBuyerDashboard")]
        public async Task<IActionResult> GetBuyerDashboard(string buyerId)
        {
            var response = await _dashboardRepository.GetBuyerDashboard(buyerId);
            return Ok(response);
        }

        [HttpGet("Seller/Earnings")]
        public async Task<IActionResult> GetSellerEarnings(string sellerId)
        {
            var response = await _dashboardRepository.GetSellerDashboard(sellerId);
            return Ok(response);
        }

        [HttpGet("Seller/Returns")]
        public async Task<IActionResult> GetSellerReturns(string sellerId)
        {
            var response = await _dashboardRepository.GetSellerDashboard(sellerId);
            return Ok(response);
        }
    }
}
