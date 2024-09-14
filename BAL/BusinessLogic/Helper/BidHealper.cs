using BAL.BusinessLogic.Interface;
using DAL;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL.Models;
using BAL.RequestModels;
using BAL.ResponseModels;
using BAL.Common;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data;
using MySql.Data.MySqlClient;



namespace BAL.BusinessLogic.Helper
{
    public class BidHealper : IBidHealper
    {
        private readonly IsqlDataHelper _isqlDataHelper;
        private readonly string _exPathToSave;
        private readonly IConfiguration _configuration;
        private readonly S3Helper _s3Helper;

        public BidHealper(IConfiguration configuration, IsqlDataHelper isqlDataHelper)
        {
            _s3Helper = new S3Helper(configuration);
            _configuration = configuration;
            _isqlDataHelper = isqlDataHelper;
            _exPathToSave = Path.Combine(Directory.GetCurrentDirectory(), "BidExceptionLogs");
        }

        public async Task<Response<BidResponse>> AddBid(Bid bid)
        {
            Response<BidResponse> response = new Response<BidResponse>();
            try
            {
                MySqlCommand cmdbid = new MySqlCommand(StoredProcedures.ADD_UPDATE_BID);
                cmdbid.CommandType = CommandType.StoredProcedure;
                cmdbid.Parameters.AddWithValue("@p_BidId", bid.BidId);
                cmdbid.Parameters.AddWithValue("@p_BuyerId", bid.BuyerId);
                cmdbid.Parameters.AddWithValue("@p_ProductId", bid.ProductId);
                cmdbid.Parameters.AddWithValue("@p_Price", bid.Price);
                cmdbid.Parameters.AddWithValue("@p_Quantity", bid.Quantity);
                cmdbid.Parameters.AddWithValue("@p_Comments", bid.Comments);
                cmdbid.Parameters.AddWithValue("@p_StatusId", bid.StatusId);
                cmdbid.Parameters.AddWithValue("@p_IsActive", bid.IsActive);
                cmdbid.Parameters.AddWithValue("@p_CreatedOn", bid.CreatedOn);
                DataTable tblbid = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmdbid));

                var objbid = new BidResponse();

                if (tblbid?.Rows.Count > 0)
                {
                    objbid.BidId = tblbid.Rows[0]["BidId"].ToString() ?? "";
                    objbid.BuyerId = tblbid.Rows[0]["BuyerId"].ToString() ?? "";
                    objbid.ProductId = tblbid.Rows[0]["ProductId"].ToString() ?? "";
                    objbid.Price = Convert.ToDecimal(tblbid.Rows[0]["Price"] ?? 0.0);
                    objbid.Quantity = Convert.ToInt32(tblbid.Rows[0]["Quantity"] ?? 0);
                    objbid.Comments = tblbid.Rows[0]["Comments"].ToString() ?? "";
                    objbid.StatusId = Convert.ToInt32(tblbid.Rows[0]["StatusId"] ?? 0);
                    objbid.IsActive = Convert.ToInt32(tblbid.Rows[0]["IsActive"] ?? 0) == 1 ? true : false;
                    objbid.CreatedOn = Convert.ToDateTime(tblbid.Rows[0]["CreatedOn"] ?? DateTime.MinValue);

                }
                response.StatusCode = 200;
                response.Message = "Bid  Added Successfully.";
                response.Result = new List<BidResponse>() { objbid };
            }
            catch(Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;

            }
            return response;
        }
        public async Task<Response<BidResponse>> UpdateBid(Bid bid)
        {
            Response<BidResponse> response = new Response<BidResponse>();
            try
            {
                MySqlCommand cmdbid = new MySqlCommand(StoredProcedures.ADD_UPDATE_BID);
                cmdbid.CommandType = CommandType.StoredProcedure;
                cmdbid.Parameters.AddWithValue("@p_BidId", bid.BidId);
                cmdbid.Parameters.AddWithValue("@p_BuyerId", bid.BuyerId);
                cmdbid.Parameters.AddWithValue("@p_ProductId", bid.ProductId);
                cmdbid.Parameters.AddWithValue("@p_Price", bid.Price);
                cmdbid.Parameters.AddWithValue("@p_Quantity", bid.Quantity);
                cmdbid.Parameters.AddWithValue("@p_Comments", bid.Comments);
                cmdbid.Parameters.AddWithValue("@p_StatusId", bid.StatusId);
                cmdbid.Parameters.AddWithValue("@p_IsActive", bid.IsActive);
                cmdbid.Parameters.AddWithValue("@p_CreatedOn", bid.CreatedOn);
                DataTable tblbid = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmdbid));

                var objbid = new BidResponse();

                if (tblbid?.Rows.Count > 0)
                {
                    objbid.BidId = tblbid.Rows[0]["BidId"].ToString() ?? "";
                    objbid.BuyerId = tblbid.Rows[0]["BuyerId"].ToString() ?? "";
                    objbid.ProductId = tblbid.Rows[0]["ProductId"].ToString() ?? "";
                    objbid.Price = Convert.ToDecimal(tblbid.Rows[0]["Price"] ?? 0.0);
                    objbid.Quantity = Convert.ToInt32(tblbid.Rows[0]["Quantity"] ?? 0);
                    objbid.Comments = tblbid.Rows[0]["Comments"].ToString() ?? "";
                    objbid.StatusId = Convert.ToInt32(tblbid.Rows[0]["StatusId"] ?? 0);
                    objbid.IsActive = Convert.ToInt32(tblbid.Rows[0]["IsActive"] ?? 0) == 1 ? true : false;
                    objbid.CreatedOn = Convert.ToDateTime(tblbid.Rows[0]["CreatedOn"] ?? DateTime.MinValue);

                }
                response.StatusCode = 200;
                response.Message = "Bid  Updated Successfully.";
                response.Result = new List<BidResponse>() { objbid };

            }
            catch(Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return response;

        }

        public async  Task<Response<BidResponse>> GetBidsByBuyer(string BidId = null)
        {
            Response<BidResponse> response = new Response<BidResponse>();
            try
            {
                MySqlCommand bidsbybuyer = new MySqlCommand(StoredProcedures.GET_BIDS_BY_BUYER);
                bidsbybuyer.CommandType = CommandType.StoredProcedure;

                bidsbybuyer.Parameters.AddWithValue("p_BuyerId", BidId);

                DataTable bids = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(bidsbybuyer));

                response.StatusCode = 200;
                response.Message = "SUCCESS : Fetch Data";
                response.Result = MapDataTableToBidList(bids);
            }
            catch (Exception ex)
            {
                
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Response<BidResponse>> GetBidsByProduct(string productId)
        {

            Response<BidResponse> response = new Response<BidResponse>();
            try
            {
                MySqlCommand bidsbyproduct = new MySqlCommand(StoredProcedures.GET_BIDS_BY_PRODUCT);
                bidsbyproduct.CommandType = CommandType.StoredProcedure;

                bidsbyproduct.Parameters.AddWithValue("p_ProductId", productId);

                DataTable bids = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(bidsbyproduct));

                response.StatusCode = 200;
                response.Message = "SUCCESS : Fetch Data";
                response.Result = MapDataTableToBidList(bids);
            }
            catch (Exception ex)
            {

                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return response;

        }

        public async Task<Response<BidResponse>> GetBidsBySeller(string sellerId = null)
        {
            Response<BidResponse> response = new Response<BidResponse>();
            try
            {
                MySqlCommand bidsbyseller = new MySqlCommand(StoredProcedures.GET_BIDS_BY_SELLER);
                bidsbyseller.CommandType = CommandType.StoredProcedure;

                bidsbyseller.Parameters.AddWithValue("p_SellerId", sellerId);

                DataTable bids = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(bidsbyseller));

                response.StatusCode = 200;
                response.Message = "SUCCESS : Fetch Data";
                response.Result = MapDataTableToBidList(bids);
            }
            catch (Exception ex)
            {

                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Response<BidResponse>> GetProductsQuotesByBuyer(string BuyerId)
        {
            Response<BidResponse> response = new Response<BidResponse>();
            try
            {
                MySqlCommand productquotesbuyer = new MySqlCommand(StoredProcedures.GET_PRODUCTS_QUOTED_BY_BUYER);
                productquotesbuyer.CommandType = CommandType.StoredProcedure;

                productquotesbuyer.Parameters.AddWithValue("p_BuyerId", BuyerId);

                DataTable bids = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(productquotesbuyer));

                response.StatusCode = 200;
                response.Message = "SUCCESS : Fetch Data";
                response.Result = MapDataTableToBidList(bids);
            }
            catch (Exception ex)
            {

                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Response<BidResponse>> GetProductsQuotesBySeller(string sellerId)
        {
            Response<BidResponse> response = new Response<BidResponse>();
            try
            {
                MySqlCommand productquotesseller = new MySqlCommand(StoredProcedures.GET_PRODUCTS_QUOTED_BY_SELLER);
                productquotesseller.CommandType = CommandType.StoredProcedure;

                productquotesseller.Parameters.AddWithValue("p_SellerId", sellerId);

                DataTable bids = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(productquotesseller));

                response.StatusCode = 200;
                response.Message = "SUCCESS : Fetch Data";
                response.Result = MapDataTableToBidList(bids);
            }
            catch (Exception ex)
            {

                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Response<string>> RemoveBid(string bidId)
        {

            Response<string> response = new Response<string>();
            try
            {
                MySqlCommand cmdRemovebid = new MySqlCommand(StoredProcedures.REMOVE_BID);
                cmdRemovebid.CommandType = CommandType.StoredProcedure;

                cmdRemovebid.Parameters.AddWithValue("", bidId);
                MySqlParameter outMessageParam = new MySqlParameter("", MySqlDbType.String)
                {
                    Direction = ParameterDirection.Output
                };
                cmdRemovebid.Parameters.Add(outMessageParam);

                await _isqlDataHelper.ExcuteNonQueryasync(cmdRemovebid);

                response.StatusCode = 200;
                response.Message = "SUCCESS : Command Execution";
                response.Result = new List<string>() { outMessageParam.Value.ToString() ?? "" };
            }
            catch(Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return response;

        }
        private static List<BidResponse> MapDataTableToBidList(DataTable tblbid)
        {
            List<BidResponse> lstbids = new List<BidResponse>();
            foreach (DataRow bid in tblbid.Rows)
            {
                BidResponse item = new BidResponse();
                item.BidId = bid["BidId"].ToString() ?? "";
                item.BuyerId = bid["BuyerId"].ToString() ?? "";
                item.ProductId = bid["ProductId"].ToString() ?? "";
                item.Price = Convert.ToDecimal(Convert.IsDBNull(bid["Price"]) ? 0.0 : bid["Price"]);
                item.Quantity = Convert.ToInt32(Convert.IsDBNull(bid["Quantity"]) ? 0 : bid["Quantity"]);
                item.Comments = bid["Comments"].ToString() ?? "";
                item.StatusId = Convert.ToInt32(bid["StatusId"] != DBNull.Value ? bid["StatusId"] : 0);
                item.IsActive = Convert.ToInt32(Convert.IsDBNull(bid["IsActive"]) ? 0 : bid["IsActive"]) == 1 ? true : false;
                item.CreatedOn = Convert.ToDateTime(tblbid.Rows[0]["CreatedOn"] ?? DateTime.MinValue);
                lstbids.Add(item);
            }
            return lstbids;
        }


    }
}
