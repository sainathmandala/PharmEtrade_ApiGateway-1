using BAL.BusinessLogic.Interface;
using BAL.Common;
using BAL.Models.FedEx;
using BAL.Models.Reports;
using DAL;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using OfficeOpenXml.Table.PivotTable;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace BAL.BusinessLogic.Helper
{
    public class ReportsHelper : IReportsHelper
    {
        private readonly IsqlDataHelper _isqlDataHelper;
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        private readonly IEmailHelper _emailHelper;
        private readonly ISquareupHelper _squareupHelper;

        public ReportsHelper(IConfiguration configuration, IsqlDataHelper isqlDataHelper, IEmailHelper emailHelper, ISquareupHelper squareupHelper)
        {
            _configuration = configuration;
            _isqlDataHelper = isqlDataHelper;
            _connectionString = configuration.GetConnectionString("APIDBConnectionString") ?? "";
            _emailHelper = emailHelper;
            _squareupHelper = squareupHelper;
        }

        public async Task<ReportResponse<ExpiredItemsReportRecord>> GenerateExpiredItemsReport(DateTime? fromDate, DateTime? toDate)
        {
            var result = new ReportResponse<ExpiredItemsReportRecord>();
            await Task.Run(() => result = default);
            return result;
        }

        public async Task<ReportResponse<NewOrdersReportRecord>> GenerateNewOrdersReport(DateTime? fromDate, DateTime? toDate)
        {
            var result = new ReportResponse<NewOrdersReportRecord>();
            await Task.Run(() => result = default);
            return result;
        }

        public async Task<ReportResponse<PaymentHistoryReportRecord>> GeneratePaymentHistoryReport(DateTime? fromDate, DateTime? toDate)
        {
            var response = new ReportResponse<PaymentHistoryReportRecord>();
            string storedProcedureName = "sp_GeneratePaymentHistory";            
            try
            {
                using (MySqlCommand dbCommand = new MySqlCommand())
                {
                    dbCommand.CommandText = storedProcedureName;
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.AddWithValue("@p_fromDate", fromDate);
                    dbCommand.Parameters.AddWithValue("@p_toDate", toDate);
                    DataTable tblOrders = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(dbCommand));
                    response.StatusCode = 200;
                    response.Message = "Successfully fetched data";
                    response.ResultData = MapTableToObject(tblOrders);
                }
            }
            catch (Exception ex)
            {

            }
            return response;
        }

        public async Task<ReportResponse<PendingShipmentsReportRecord>> GeneratePendingShipmentsReport(DateTime? fromDate, DateTime? toDate)
        {
            var result = new ReportResponse<PendingShipmentsReportRecord>();
            await Task.Run(() => result = default);
            return result;
        }

        public async Task<ReportResponse<PurchaseHistoryReportRecord>> GeneratePurchaseHistoryReport(DateTime? fromDate, DateTime? toDate)
        {
            var rObj = new PurchaseHistoryReportRecord();
            DataTable dt = new DataTable();

            var properties = typeof(PurchaseHistoryReportRecord).GetType().GetProperties();
            foreach (var prop in properties)
            {
                dt.Columns.Add(new DataColumn(prop.Name));
            }


            DataRow dataRow = dt.NewRow();
            int counter = 0;
            foreach (var prop in properties)
            {
                dataRow[prop.Name] = prop.Name + counter++;
            }

            foreach (var prop in properties)
            {
                if (prop.GetSetMethod() != null)
                {
                    prop.SetValue(this, dataRow[prop.Name]);
                };
            }
            //
            var result = new ReportResponse<PurchaseHistoryReportRecord>();
            await Task.Run(() => result.ResultData = default);
            return result;
        }

        public async Task<ReportResponse<PaymentHistoryReportRecord>> RunReport(int reportType, DateTime? fromDate, DateTime? toDate)
        {
            var response = new ReportResponse<PaymentHistoryReportRecord>();
            string storedProcedureName = "";
            switch(reportType)
            {
                case (int)ReportTypes.PAYMENT_HISTORY:
                    storedProcedureName = "sp_GeneratePaymentHistory";
                    break;
                case (int)ReportTypes.PURCHASE_HISTORY:
                    storedProcedureName = "sp_GeneratePaymentHistory";
                    break;
                case (int)ReportTypes.NEW_ORDERS:
                    storedProcedureName = "sp_GeneratePaymentHistory";
                    break;
                case (int)ReportTypes.EXPIRED_ITEMS:
                    storedProcedureName = "sp_GeneratePaymentHistory";
                    break;
                case (int)ReportTypes.PENDING_SHIPMENTS:
                    storedProcedureName = "sp_GeneratePaymentHistory";
                    break;
                default:
                    storedProcedureName = "sp_GeneratePaymentHistory";
                    break;
            }
            try
            {
                using(MySqlCommand dbCommand = new MySqlCommand())
                {
                    dbCommand.CommandText = storedProcedureName;
                    dbCommand.CommandType = CommandType.StoredProcedure;
                    dbCommand.Parameters.AddWithValue("@p_fromDate", fromDate);
                    dbCommand.Parameters.AddWithValue("@p_toDate", toDate);
                    DataTable tblOrders = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(dbCommand));
                    response.StatusCode = 200;
                    response.Message = "Successfully fetched data";
                    response.ResultData = MapTableToObject(tblOrders);
                }
            }
            catch (Exception ex)
            {

            }
            return response;
        }
        private List<PaymentHistoryReportRecord> MapTableToObject(DataTable tblOrders)
        {
            var result = new List<PaymentHistoryReportRecord>();
            if(tblOrders == null || tblOrders.Rows.Count <= 0) { return result; }
            foreach (DataRow row in tblOrders.Rows)
            {
                PaymentHistoryReportRecord phrRecord = new PaymentHistoryReportRecord();
                phrRecord.OrderNumber = row["OrderNumber"].ToString() ?? "";
                phrRecord.OrderDate = Convert.ToDateTime(row["OrderDate"] == DBNull.Value ? null : row["OrderDate"]);
                phrRecord.PaymentDate = Convert.ToDateTime(row["PaymentDate"] == DBNull.Value ? null : row["PaymentDate"]);
                phrRecord.PaymentMethod = row["PaymentMethod"].ToString() ?? "";
                phrRecord.PaymentAmount = Convert.ToDecimal( row["PaymentAmount"] == DBNull.Value ? 0.0 : row["PaymentAmount"]);
                phrRecord.CustomerName = row["CustomerName"].ToString() ?? "";

                result.Add(phrRecord);
            }
            return result;
        }
        private List<T> MapTableToObject<T>(DataTable table, T toObject)
        {
            var result = new List<T>();
            foreach (DataRow row in table.Rows)
            {
                var propertyInfoArray = toObject.GetType().GetProperties(
                                    BindingFlags.Public |
                                    BindingFlags.Instance);
                foreach (var propertyInfo in propertyInfoArray)
                {
                    var propValue = propertyInfo.GetValue(toObject, null);
                    if (propValue == null)
                        continue;
                    if (propValue.GetType().Name == "String")
                        propertyInfo.SetValue(
                                         toObject,
                                         ((string)propValue).Trim(),
                                         null);
                }
                result.Add(toObject);
            }
            return result;
        }
    }
}
