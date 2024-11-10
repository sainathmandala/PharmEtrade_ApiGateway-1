using BAL.BusinessLogic.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmEtrade_ApiGateway.Repository.Interface;

namespace PharmEtrade_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private IReportsRepository _reportsRepository;        
        public ReportsController(IReportsRepository reportsRepository)
        {
            _reportsRepository = reportsRepository;
        }

        [HttpGet("Generate")]
        public async Task<IActionResult> GenerateReport(int reportType, DateTime? fromDate, DateTime? toDate)
        {
            var reportResponse = await _reportsRepository.RunReport(reportType, fromDate, toDate);
            return Ok(reportResponse);
        }

        [HttpGet("PaymentHistory")]
        public async Task<IActionResult> GeneratePaymentHistory(DateTime? fromDate, DateTime? toDate)
        {
            var reportResponse = await _reportsRepository.GeneratePaymentHistoryReport(fromDate, toDate);
            return Ok(reportResponse);
        }

        [HttpGet("PurchaseHistory")]
        public async Task<IActionResult> GeneratePurchaseHistory(DateTime? fromDate, DateTime? toDate)
        {
            var reportResponse = await _reportsRepository.GeneratePurchaseHistoryReport(fromDate, toDate);
            return Ok(reportResponse);
        }

        [HttpGet("NewOrders")]
        public async Task<IActionResult> GenerateNewOrders(DateTime? fromDate, DateTime? toDate)
        {
            var reportResponse = await _reportsRepository.GenerateNewOrdersReport(fromDate, toDate);
            return Ok(reportResponse);
        }

        [HttpGet("ExpiredItems")]
        public async Task<IActionResult> GenerateExpiredItems(DateTime? fromDate, DateTime? toDate)
        {
            var reportResponse = await _reportsRepository.GenerateExpiredItemsReport(fromDate, toDate);
            return Ok(reportResponse);
        }

        [HttpGet("PendingShipments")]
        public async Task<IActionResult> GeneratePendingShipments(DateTime? fromDate, DateTime? toDate)
        {
            var reportResponse = await _reportsRepository.GeneratePendingShipmentsReport(fromDate, toDate);
            return Ok(reportResponse);
        }
    }
}
