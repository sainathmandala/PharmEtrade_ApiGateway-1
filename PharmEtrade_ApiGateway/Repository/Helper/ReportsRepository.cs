using Amazon.Runtime.Internal;
using BAL.BusinessLogic.Interface;
using BAL.Models.Reports;
using PharmEtrade_ApiGateway.Repository.Interface;
using System.Data;

namespace PharmEtrade_ApiGateway.Repository.Helper
{
    public class ReportsRepository : IReportsRepository
    {
        private readonly IReportsHelper _reportsHelper;
        public ReportsRepository(IReportsHelper reportsHelper)
        {
            _reportsHelper = reportsHelper;
        }

        public async Task<ReportResponse<ExpiredItemsReportRecord>> GenerateExpiredItemsReport(DateTime? fromDate, DateTime? toDate)
        {
            return await _reportsHelper.GenerateExpiredItemsReport(fromDate, toDate);
        }

        public async Task<ReportResponse<NewOrdersReportRecord>> GenerateNewOrdersReport(DateTime? fromDate, DateTime? toDate)
        {
            return await _reportsHelper.GenerateNewOrdersReport(fromDate, toDate);
        }

        public async Task<ReportResponse<PaymentHistoryReportRecord>> GeneratePaymentHistoryReport(DateTime? fromDate, DateTime? toDate)
        {
            return await _reportsHelper.GeneratePaymentHistoryReport(fromDate, toDate);
        }

        public async Task<ReportResponse<PendingShipmentsReportRecord>> GeneratePendingShipmentsReport(DateTime? fromDate, DateTime? toDate)
        {
            return await _reportsHelper.GeneratePendingShipmentsReport(fromDate, toDate);
        }

        public async Task<ReportResponse<PurchaseHistoryReportRecord>> GeneratePurchaseHistoryReport(DateTime? fromDate, DateTime? toDate)
        {
            return await _reportsHelper.GeneratePurchaseHistoryReport(fromDate, toDate);
        }

        public async Task<ReportResponse<PaymentHistoryReportRecord>> RunReport(int reportType, DateTime? fromDate, DateTime? toDate)
        {
            return await _reportsHelper.RunReport(reportType, fromDate, toDate);
        }
    }
}
