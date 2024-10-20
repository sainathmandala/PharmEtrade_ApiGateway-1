using Amazon.Runtime.Internal.Transform;
using BAL.BusinessLogic.Interface;
using BAL.Common;
using BAL.ResponseModels;
using DAL;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.BusinessLogic.Helper
{
    public class DashboardHelper : IDashboardHelper
    {
        private readonly IsqlDataHelper _isqlDataHelper;       

        public DashboardHelper(IsqlDataHelper isqlDataHelper) { 
            _isqlDataHelper = isqlDataHelper;
        }
        public async Task<AdminDashboardResponse> GetAdminDashboard(string adminId)
        {
            var response = new AdminDashboardResponse();
            using (MySqlCommand command = new MySqlCommand(StoredProcedures.DASHBOARD_GET_BY_ADMIN))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_AdminId", adminId);

                try
                {
                    DataTable tblAdminDashboard = await _isqlDataHelper.ExecuteDataTableAsync(command);
                    if (tblAdminDashboard != null && tblAdminDashboard.Rows.Count > 0)
                    {
                        response.StatusCode = "SUCCESS".Equals(tblAdminDashboard.Rows[0]["Message"].ToString() ?? "") ? 200 : 500;
                        response.Message = tblAdminDashboard.Rows[0]["Message"].ToString() ?? "";
                        //response.TotalCustomers = Convert.ToInt32(tblAdminDashboard.Rows[0]["TotalCustomers"] == DBNull.Value ? 0 : tblAdminDashboard.Rows[0]["TotalCustomers"]);
                        response.TotalOrders = Convert.ToInt32(tblAdminDashboard.Rows[0]["TotalOrders"] == DBNull.Value ? 0 : tblAdminDashboard.Rows[0]["TotalOrders"]);
                        response.TotalProducts = Convert.ToInt32(tblAdminDashboard.Rows[0]["TotalProducts"] == DBNull.Value ? 0 : tblAdminDashboard.Rows[0]["TotalProducts"]);
                        response.TotalActiveProducts = Convert.ToInt32(tblAdminDashboard.Rows[0]["TotalActiveProducts"] == DBNull.Value ? 0 : tblAdminDashboard.Rows[0]["TotalActiveProducts"]);
                        response.TotalInActiveProducts = Convert.ToInt32(tblAdminDashboard.Rows[0]["TotalInActiveProducts"] == DBNull.Value ? 0 : tblAdminDashboard.Rows[0]["TotalInActiveProducts"]);
                        var CustomerCountsPerType = tblAdminDashboard.Rows[0]["CustomerCountsPerType"].ToString() ?? "";
                        // response.CountsPerTypes = new Dictionary<string, string>();
                        foreach (var item in CustomerCountsPerType.Split("|"))
                        {
                            if(string.IsNullOrEmpty(item)) continue;
                            response.CustomersCounts.Add(new CustomersCount() {
                                CustomerTypeId = Convert.ToInt32(item.Split(":")[0]),                                
                                Count = Convert.ToInt32(item.Split(":")[1]),
                                ActiveCount = Convert.ToInt32(item.Split(":")[2]),
                                InActiveCount = Convert.ToInt32(item.Split(":")[3])
                            });
                        }
                        response.TotalCustomers = response.CustomersCounts.Sum(c => c.Count);
                        response.TotalActiveCustomers = response.CustomersCounts.Sum(c => c.ActiveCount);
                        response.TotalInActiveCustomers = response.CustomersCounts.Sum(c => c.InActiveCount);
                    }
                    else
                    {
                        response.StatusCode = 500;
                        response.Message = "No records found for the Buyer";
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = 500;
                    response.Message = "ERROR : " + ex.Message;
                }
            }

            return response;
        }

        public async Task<BuyerDashboardResponse> GetBuyerDashboard(string buyerId)
        {
            var response = new BuyerDashboardResponse();
            using (MySqlCommand command = new MySqlCommand(StoredProcedures.DASHBOARD_GET_BY_BUYER))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_BuyerId", buyerId);

                try
                {
                    DataTable tblBuyerDashboard = await _isqlDataHelper.ExecuteDataTableAsync(command);
                    if (tblBuyerDashboard != null && tblBuyerDashboard.Rows.Count > 0)
                    {
                        response.StatusCode = "SUCCESS".Equals(tblBuyerDashboard.Rows[0]["Message"].ToString() ?? "") ? 200 : 500;
                        response.Message = tblBuyerDashboard.Rows[0]["Message"].ToString() ?? "";
                        response.ProductsOrdered = Convert.ToInt32(tblBuyerDashboard.Rows[0]["ProductsOrdered"] == DBNull.Value ? 0 : tblBuyerDashboard.Rows[0]["ProductsOrdered"]);
                        response.TotalOrders = Convert.ToInt32(tblBuyerDashboard.Rows[0]["TotalOrders"] == DBNull.Value ? 0 : tblBuyerDashboard.Rows[0]["TotalOrders"]);
                        response.UpcomingOrders = Convert.ToInt32(tblBuyerDashboard.Rows[0]["UpcomingOrders"] == DBNull.Value ? 0 : tblBuyerDashboard.Rows[0]["UpcomingOrders"]);
                        response.ReceivedOrders = Convert.ToInt32(tblBuyerDashboard.Rows[0]["ReceivedOrders"] == DBNull.Value ? 0 : tblBuyerDashboard.Rows[0]["ReceivedOrders"]);
                        response.CancelledOrders = Convert.ToInt32(tblBuyerDashboard.Rows[0]["CancelledOrders"] == DBNull.Value ? 0 : tblBuyerDashboard.Rows[0]["CancelledOrders"]);
                        response.WishList = Convert.ToInt32(tblBuyerDashboard.Rows[0]["WishList"] == DBNull.Value ? 0 : tblBuyerDashboard.Rows[0]["WishList"]);
                        response.TotalPurchaseValue = Convert.ToDecimal(tblBuyerDashboard.Rows[0]["TotalPurchaseValue"] == DBNull.Value ? 0.0 : tblBuyerDashboard.Rows[0]["TotalPurchaseValue"]);
                    }
                    else
                    {
                        response.StatusCode = 500;
                        response.Message = "No records found for the Buyer";
                    }
                }
                catch(Exception ex)
                {
                    response.StatusCode = 500;
                    response.Message = "ERROR : " + ex.Message;                    
                }
            }

            return response;
        }

        public async Task<SellerDashboardResponse> GetSellerDashboard(string sellerId)
        {
            var response = new SellerDashboardResponse();
            using (MySqlCommand command = new MySqlCommand(StoredProcedures.DASHBOARD_GET_BY_SELLER))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_SellerId", sellerId);

                try
                {
                    DataTable tblSellerDashboard = await _isqlDataHelper.ExecuteDataTableAsync(command);
                    if (tblSellerDashboard != null && tblSellerDashboard.Rows.Count > 0)
                    {
                        response.StatusCode = "SUCCESS".Equals(tblSellerDashboard.Rows[0]["Message"].ToString() ?? "") ? 200 : 500;
                        response.Message = tblSellerDashboard.Rows[0]["Message"].ToString() ?? "";
                        response.ProductsOrdered = Convert.ToInt32(tblSellerDashboard.Rows[0]["ProductsOrdered"] == DBNull.Value ? 0 : tblSellerDashboard.Rows[0]["ProductsOrdered"]);
                        response.TotalOrders = Convert.ToInt32(tblSellerDashboard.Rows[0]["TotalOrders"] == DBNull.Value ? 0 : tblSellerDashboard.Rows[0]["TotalOrders"]);
                        response.CustomersOrdered = Convert.ToInt32(tblSellerDashboard.Rows[0]["CustomersOrdered"] == DBNull.Value ? 0 : tblSellerDashboard.Rows[0]["CustomersOrdered"]);
                        response.TotalSaleValue = Convert.ToDecimal(tblSellerDashboard.Rows[0]["TotalSaleValue"] == DBNull.Value ? 0.0 : tblSellerDashboard.Rows[0]["TotalSaleValue"]);
                        response.TotalProducts = Convert.ToInt32(tblSellerDashboard.Rows[0]["TotalProducts"] == DBNull.Value ? 0 : tblSellerDashboard.Rows[0]["TotalProducts"]);
                        response.ActiveProducts = Convert.ToInt32(tblSellerDashboard.Rows[0]["ActiveProducts"] == DBNull.Value ? 0 : tblSellerDashboard.Rows[0]["ActiveProducts"]);
                        response.OutgoingOrdersCount = Convert.ToInt32(tblSellerDashboard.Rows[0]["OutgoingOrdersCount"] == DBNull.Value ? 0 : tblSellerDashboard.Rows[0]["OutgoingOrdersCount"]);
                        response.TotalPurchaseValue = Convert.ToDecimal(tblSellerDashboard.Rows[0]["TotalPurchaseValue"] == DBNull.Value ? 0.0 : tblSellerDashboard.Rows[0]["TotalPurchaseValue"]);
                    }
                    else
                    {
                        response.StatusCode = 500;
                        response.Message = "No records found for the Buyer";                        
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = 500;
                    response.Message = "ERROR : " + ex.Message;                    
                }
            }

            return response;
        }
    }
}
