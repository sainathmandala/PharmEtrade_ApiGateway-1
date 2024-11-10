using BAL.Models.Reports;
using System.Data;

namespace BAL.BusinessLogic.Interface
{
    public interface IReportsHelper
    {
        Task<ReportResponse<PaymentHistoryReportRecord>> GeneratePaymentHistoryReport(DateTime? fromDate, DateTime? toDate);
        Task<ReportResponse<PurchaseHistoryReportRecord>> GeneratePurchaseHistoryReport(DateTime? fromDate, DateTime? toDate);
        Task<ReportResponse<NewOrdersReportRecord>> GenerateNewOrdersReport(DateTime? fromDate, DateTime? toDate);
        Task<ReportResponse<ExpiredItemsReportRecord>> GenerateExpiredItemsReport(DateTime? fromDate, DateTime? toDate);
        Task<ReportResponse<PendingShipmentsReportRecord>> GeneratePendingShipmentsReport(DateTime? fromDate, DateTime? toDate);
        Task<ReportResponse<PaymentHistoryReportRecord>> RunReport(int reportType, DateTime? fromDate, DateTime? toDate);
    }
}
