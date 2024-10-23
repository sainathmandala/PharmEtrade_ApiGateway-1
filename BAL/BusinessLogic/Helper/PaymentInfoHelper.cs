using DAL;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL.BusinessLogic.Interface;
using BAL.Models;
using Microsoft.AspNetCore.Http;
using BAL.RequestModels;
using System.Configuration;
using BAL.Common;
using System.Data;
using BAL.ResponseModels;
using MySql.Data.MySqlClient;
using BAL.ViewModels;

namespace BAL.BusinessLogic.Helper
{
    public class PaymentInfoHelper : IPaymentInfo
    {

        private readonly IsqlDataHelper _isqlDataHelper;
        private readonly string _exPathToSave;
        private readonly IConfiguration _configuration;
        private readonly S3Helper _s3Helper;
        public PaymentInfoHelper(IConfiguration configuration, IsqlDataHelper isqlDataHelper)
        {
            _s3Helper = new S3Helper(configuration);
            _configuration = configuration;
            _isqlDataHelper = isqlDataHelper;
            _exPathToSave = Path.Combine(Directory.GetCurrentDirectory(), "ProductExceptionLogs");
        }

        public async Task<Response<PaymentInfo>> AddPayment(PaymentInfo paymentInfo)
        {
            Response<PaymentInfo> response = new Response<PaymentInfo>();
            try
            {
                MySqlCommand cmdpaymentInfo = new MySqlCommand(StoredProcedures.ADD_UPDATE_PAYMENTINFO);
                cmdpaymentInfo.CommandType = CommandType.StoredProcedure;

                cmdpaymentInfo.Parameters.AddWithValue("@p_PaymentInfoId",paymentInfo.PaymentInfoId);
                cmdpaymentInfo.Parameters.AddWithValue("@p_OrderId", paymentInfo.OrderId);
                cmdpaymentInfo.Parameters.AddWithValue("@p_PaymentMethodId", paymentInfo.PaymentMethodId);
                cmdpaymentInfo.Parameters.AddWithValue("@p_CardNumber", paymentInfo.CardNumber);
                cmdpaymentInfo.Parameters.AddWithValue("@p_CardType", paymentInfo.CardType);
                cmdpaymentInfo.Parameters.AddWithValue("@p_CVV", paymentInfo.CVV);
                cmdpaymentInfo.Parameters.AddWithValue("@p_ValidThrough", paymentInfo.ValidThrough);
                cmdpaymentInfo.Parameters.AddWithValue("@p_NameOnCard", paymentInfo.NameOnCard);
                cmdpaymentInfo.Parameters.AddWithValue("@p_Bank", paymentInfo.Bank);
                cmdpaymentInfo.Parameters.AddWithValue("@p_PaymentAmount", paymentInfo.PaymentAmount);
                cmdpaymentInfo.Parameters.AddWithValue("@p_IsCreditCard", paymentInfo.IsCreditCard);
                cmdpaymentInfo.Parameters.AddWithValue("@p_StatusId", paymentInfo.StatusId);
                cmdpaymentInfo.Parameters.AddWithValue("@p_PaymentDate", paymentInfo.PaymentDate);
                

                DataTable tblPaymentinfo = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmdpaymentInfo));

                var objPaymentInfo = new Models.PaymentInfo();
                if ( tblPaymentinfo?.Rows.Count > 0)
                {
                    objPaymentInfo.PaymentInfoId = tblPaymentinfo.Rows[0]["PaymentInfoId"].ToString() ?? "";
                    objPaymentInfo.OrderId = tblPaymentinfo.Rows[0]["OrderId"].ToString()??"";
                    objPaymentInfo.PaymentMethodId =Convert.ToInt32(tblPaymentinfo.Rows[0]["PaymentMethodId"] ?? 0);
                    objPaymentInfo.CardNumber = tblPaymentinfo.Rows[0]["CardNumber"].ToString() ?? "";
                    objPaymentInfo.CardType = tblPaymentinfo.Rows[0]["CardType"].ToString() ?? "";
                    objPaymentInfo.CVV = tblPaymentinfo.Rows[0]["CVV"].ToString() ?? "";
                    objPaymentInfo.ValidThrough = tblPaymentinfo.Rows[0]["ValidThrough"].ToString() ?? "";
                    objPaymentInfo.NameOnCard = tblPaymentinfo.Rows[0]["NameOnCard"].ToString() ?? "";
                    objPaymentInfo.Bank = tblPaymentinfo.Rows[0]["Bank"].ToString() ?? "";
                    objPaymentInfo.PaymentAmount = Convert.ToDecimal(tblPaymentinfo.Rows[0]["PaymentAmount"] ?? 0.0);
                    objPaymentInfo.IsCreditCard = Convert.ToInt32(tblPaymentinfo.Rows[0]["IsCreditCard"] ?? 0) == 1 ? true : false;
                    objPaymentInfo.StatusId = Convert.ToInt32(tblPaymentinfo.Rows[0]["StatusId"] ?? 0);
                    objPaymentInfo.PaymentDate = Convert.ToDateTime(tblPaymentinfo.Rows[0]["PaymentDate"] ?? DateTime.MinValue);

                }

                response.StatusCode = 200;
                response.Message = "PaymentInfo Added/Updated Successfully.";
                response.Result = new List<PaymentInfo>() { objPaymentInfo };
            }
            catch (Exception ex)
            {
                //Task writeTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "Add Update Product Size :  errormessage:" + ex.Message));
                // Handle the exception as needed
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return response;
        }
        public  async Task<Response<PaymentInfo>> UpdatePayment(PaymentInfo paymentInfo)
        {
            Response<PaymentInfo> response = new Response<PaymentInfo>();
            try
            {
                MySqlCommand cmdpaymentInfo = new MySqlCommand(StoredProcedures.ADD_UPDATE_PAYMENTINFO);
                cmdpaymentInfo.CommandType = CommandType.StoredProcedure;

                cmdpaymentInfo.Parameters.AddWithValue("@p_PaymentInfoId", paymentInfo.PaymentInfoId);
                cmdpaymentInfo.Parameters.AddWithValue("@p_OrderId", paymentInfo.OrderId);
                cmdpaymentInfo.Parameters.AddWithValue("@p_PaymentMethodId", paymentInfo.PaymentMethodId);
                cmdpaymentInfo.Parameters.AddWithValue("@p_CardNumber", paymentInfo.CardNumber);
                cmdpaymentInfo.Parameters.AddWithValue("@p_CardType", paymentInfo.CardType);
                cmdpaymentInfo.Parameters.AddWithValue("@p_CVV", paymentInfo.CVV);
                cmdpaymentInfo.Parameters.AddWithValue("@p_ValidThrough", paymentInfo.ValidThrough);
                cmdpaymentInfo.Parameters.AddWithValue("@p_NameOnCard", paymentInfo.NameOnCard);
                cmdpaymentInfo.Parameters.AddWithValue("@p_Bank", paymentInfo.Bank);
                cmdpaymentInfo.Parameters.AddWithValue("@p_PaymentAmount", paymentInfo.PaymentAmount);
                cmdpaymentInfo.Parameters.AddWithValue("@p_IsCreditCard", paymentInfo.IsCreditCard);
                cmdpaymentInfo.Parameters.AddWithValue("@p_StatusId", paymentInfo.StatusId);
                cmdpaymentInfo.Parameters.AddWithValue("@p_PaymentDate", paymentInfo.PaymentDate);


                DataTable tblPaymentinfo = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmdpaymentInfo));

                var objPaymentInfo = new Models.PaymentInfo();
                if (tblPaymentinfo?.Rows.Count > 0)
                {
                    objPaymentInfo.PaymentInfoId = tblPaymentinfo.Rows[0]["PaymentInfoId"].ToString() ?? "";
                    objPaymentInfo.OrderId = tblPaymentinfo.Rows[0]["OrderId"].ToString() ?? "";
                    objPaymentInfo.PaymentMethodId = Convert.ToInt32(tblPaymentinfo.Rows[0]["PaymentMethodId"] ?? 0);
                    objPaymentInfo.CardNumber = tblPaymentinfo.Rows[0]["CardNumber"].ToString() ?? "";
                    objPaymentInfo.CardType = tblPaymentinfo.Rows[0]["CardType"].ToString() ?? "";
                    objPaymentInfo.CVV = tblPaymentinfo.Rows[0]["CVV"].ToString() ?? "";
                    objPaymentInfo.ValidThrough = tblPaymentinfo.Rows[0]["ValidThrough"].ToString() ?? "";
                    objPaymentInfo.NameOnCard = tblPaymentinfo.Rows[0]["NameOnCard"].ToString() ?? "";
                    objPaymentInfo.Bank = tblPaymentinfo.Rows[0]["Bank"].ToString() ?? "";
                    objPaymentInfo.PaymentAmount = Convert.ToDecimal(tblPaymentinfo.Rows[0]["PaymentAmount"] ?? 0.0);
                    objPaymentInfo.IsCreditCard = Convert.ToInt32(tblPaymentinfo.Rows[0]["IsCreditCard"] ?? 0) == 1 ? true : false;
                    objPaymentInfo.StatusId = Convert.ToInt32(tblPaymentinfo.Rows[0]["StatusId"] ?? 0);
                    objPaymentInfo.PaymentDate = Convert.ToDateTime(tblPaymentinfo.Rows[0]["PaymentDate"] ?? DateTime.MinValue);

                }

                response.StatusCode = 200;
                response.Message = "PaymentInfo Added/Updated Successfully.";
                response.Result = new List<PaymentInfo>() { objPaymentInfo };
            }
            catch (Exception ex)
            {
                //Task writeTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "Add Update Product Size :  errormessage:" + ex.Message));
                // Handle the exception as needed
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Response<PaymentInfo>> GetPaymentInfoByOrderId(string OrderId)
        {
            Response<PaymentInfo> response = new Response<PaymentInfo>();
            try
            {
                MySqlCommand cmdPaymentinfo = new MySqlCommand(StoredProcedures.GET_PAYMENTINFO_BY_ORDERID);
                cmdPaymentinfo.CommandType = CommandType.StoredProcedure;
                cmdPaymentinfo.Parameters.AddWithValue("@p_OrderId", OrderId);

                DataTable tblPaymentinfo = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmdPaymentinfo));
                response.StatusCode = 200;
                response.Message = "Successfully Fetched Data.";
                response.Result = MapDataTableToPaymentInfo(tblPaymentinfo);
            }
            catch (Exception ex)
            {
                
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.Result = null;
            }
            return response;
        }

        public async Task<Response<PaymentInfo>> GetPaymentInfoByCustmoerId(string CustomerId)
        {
            Response<PaymentInfo> response = new Response<PaymentInfo>();
            try
            {
                MySqlCommand cmdPaymentinfo = new MySqlCommand(StoredProcedures.GET_PAYMENTINFO_BY_CUSTOMERID);
                cmdPaymentinfo.CommandType = CommandType.StoredProcedure;
                cmdPaymentinfo.Parameters.AddWithValue("@p_CustomerId", CustomerId);

                DataTable tblPaymentinfo = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmdPaymentinfo));
                response.StatusCode = 200;
                response.Message = "Successfully Fetched Data.";
                response.Result = MapDataTableToPaymentInfo(tblPaymentinfo);
            }
            catch (Exception ex)
            {
                
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.Result = null;
            }
            return response;
        }

        private static List<PaymentInfo> MapDataTableToPaymentInfo(DataTable tblpayment)
        {
            List<PaymentInfo> listpaymentinfo = new List<PaymentInfo>();
            foreach (DataRow paymentinfo in tblpayment.Rows)
            {
                PaymentInfo  item = new PaymentInfo();
                item.PaymentInfoId = paymentinfo["PaymentInfoId"].ToString() ?? "";
                item.InvoiceNumber = paymentinfo["InvoiceNumber"].ToString() ?? "";
                item.PaymentMethod = paymentinfo["PaymentMethod"].ToString() ?? "";
                item.OrderId = paymentinfo["OrderId"].ToString() ?? "";
                item.PaymentMethodId = Convert.ToInt32(Convert.IsDBNull(paymentinfo["PaymentMethodId"]) ? 0 : paymentinfo["PaymentMethodId"]);
                item.CardNumber = paymentinfo["CardNumber"].ToString() ?? "";
                item.CardType = paymentinfo["CardType"].ToString() ?? "";
                item.CVV = paymentinfo["CVV"].ToString() ?? "";
                item.ValidThrough = paymentinfo["ValidThrough"].ToString() ?? "";
                item.NameOnCard = paymentinfo["NameOnCard"].ToString() ?? "";
                item.Bank = paymentinfo["Bank"].ToString() ?? "";
                item.PaymentAmount = Convert.ToDecimal(Convert.IsDBNull(paymentinfo["PaymentAmount"]) ? 0.0 : paymentinfo["PaymentAmount"]);
                item.IsCreditCard= Convert.ToInt32(Convert.IsDBNull(paymentinfo["IsCreditCard"]) ? 0 : paymentinfo["IsCreditCard"]) == 1 ? true : false;
                item.StatusId = Convert.ToInt32(Convert.IsDBNull(paymentinfo["StatusId"]) ? 0 : paymentinfo["StatusId"]);
                item.PaymentDate = Convert.ToDateTime(Convert.IsDBNull(paymentinfo["PaymentDate"]) ? DateTime.MinValue : paymentinfo["PaymentDate"]);
                item.PaymentStatus = paymentinfo["PaymentStatus"].ToString() ?? "";
                item.InvoiceDate = Convert.ToDateTime(Convert.IsDBNull(paymentinfo["OrderDate"]) ? DateTime.MinValue : paymentinfo["OrderDate"]);
                item.FromUser = paymentinfo["FirstName"].ToString() ?? "" + " " + paymentinfo["LastName"].ToString() ?? "";

                listpaymentinfo.Add(item);
            }
            return listpaymentinfo;
        }

        public async Task<Response<PaymentInfo>> GetAllPayments()
        {
            Response<PaymentInfo> response = new Response<PaymentInfo>();
            try
            {
                MySqlCommand cmdPaymentinfo = new MySqlCommand(StoredProcedures.GET_ALL_PAYMENTS);
                cmdPaymentinfo.CommandType = CommandType.StoredProcedure;

                DataTable tblPaymentinfo = await Task.Run(() => _isqlDataHelper.ExecuteDataTableAsync(cmdPaymentinfo));
                response.StatusCode = 200;
                response.Message = "Successfully Fetched Data.";
                response.Result = MapDataTableToPaymentInfo(tblPaymentinfo);
            }
            catch (Exception ex)
            {

                response.StatusCode = 500;
                response.Message = ex.Message;
                response.Result = null;
            }
            return response;
        }

        public async Task<Response<PaymentInfo>> GetAllPayments(PaymentCriteria criteria)
        {
            Response<PaymentInfo> response = new Response<PaymentInfo>();
            try
            {
                MySqlCommand cmdPaymentinfo = new MySqlCommand(StoredProcedures.GET_PAYMENTS_BY_DATE);
                cmdPaymentinfo.CommandType = CommandType.StoredProcedure;

                cmdPaymentinfo.Parameters.AddWithValue("@p_FromDate", criteria.FromDate);
                cmdPaymentinfo.Parameters.AddWithValue("@p_ToDate", criteria.ToDate);

                DataTable tblPaymentinfo = await Task.Run(() => _isqlDataHelper.ExecuteDataTableAsync(cmdPaymentinfo));
                response.StatusCode = 200;
                response.Message = "Successfully Fetched Data.";
                response.Result = MapDataTableToPaymentInfo(tblPaymentinfo);
            }
            catch (Exception ex)
            {

                response.StatusCode = 500;
                response.Message = ex.Message;
                response.Result = null;
            }
            return response;
        }
    }
}
