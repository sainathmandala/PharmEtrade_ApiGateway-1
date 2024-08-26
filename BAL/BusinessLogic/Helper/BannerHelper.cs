using BAL.BusinessLogic.Interface;
using BAL.Common;
using BAL.Models;
using BAL.RequestModels;
using BAL.ResponseModels;
using DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BAL.BusinessLogic.Helper
{
    public class BannerHelper : IBannerHelper
    {
        private IConfiguration _configuration;
        private IsqlDataHelper _sqlDataHelper;
        private string ConnectionString
        {
            get
            {
                return _configuration.GetConnectionString("APIDBConnectionString") ?? "";
            }
        }
        public BannerHelper(IConfiguration configuration, IsqlDataHelper isqlDataHelper) {
            _configuration = configuration;
            _sqlDataHelper = isqlDataHelper;
        }

        public async Task<Response<Banner>> AddUpdateBanner(Banner banner)
        {
            var response = new Response<Banner>();
            using (MySqlConnection sqlcon = new MySqlConnection(ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(StoredProcedures.ADD_UPDATE_BANNER, sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_BannerId", banner.BannerId);
                    cmd.Parameters.AddWithValue("@p_ImageUrl", banner.ImageUrl);
                    cmd.Parameters.AddWithValue("@p_Text", banner.BannerText);
                    cmd.Parameters.AddWithValue("@p_IsActive", banner.IsActive);
                    cmd.Parameters.AddWithValue("@p_UploadedOn", banner.UploadedOn);
                    cmd.Parameters.AddWithValue("@p_OrderSequence", banner.OrderSequence);
                    MySqlParameter paramMessage = new MySqlParameter("@o_Message", MySqlDbType.String)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(paramMessage);

                    try
                    {
                        DataTable tblBanner = await Task.Run(() => _sqlDataHelper.SqlDataAdapterasync(cmd));

                        if (tblBanner.Rows.Count > 0)
                        {
                            response.StatusCode = 200;
                            response.Message = string.IsNullOrEmpty(paramMessage.Value.ToString()) ? "Success" : paramMessage.Value.ToString();
                            response.Result = MapDataTableToBannerList(tblBanner);
                        }
                        else
                        {
                            response.StatusCode = 400;
                            response.Message = "Failed to add/update banner.";
                            response.Result = null;
                        }
                    }
                    catch (MySqlException ex) when (ex.Number == 500)
                    {
                        //Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertAddToCartProduct : errormessage:" + ex.Message.ToString()));
                        response.StatusCode = 500;
                        response.Message = "ERROR : " + ex.Message;
                    }
                    catch (Exception ex)
                    {
                        //Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertAddToCartProduct : errormessage:" + ex.Message.ToString()));
                        response.StatusCode = 500;
                        response.Message = "ERROR : " + ex.Message;
                    }
                    return response;
                }
            }
        }

        public async Task<Response<Banner>> DeleteBanner(int bannerId)
        {
            var response = new Response<Banner>();
            using (MySqlConnection sqlcon = new MySqlConnection(ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(StoredProcedures.DELETE_BANNER, sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_BannerId", bannerId);
                    MySqlParameter paramMessage = new MySqlParameter("@o_Message", MySqlDbType.String)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(paramMessage);

                    try
                    {
                        DataTable tblBanner = await Task.Run(() => _sqlDataHelper.SqlDataAdapterasync(cmd));

                        if (tblBanner.Rows.Count > 0)
                        {
                            response.StatusCode = 200;
                            response.Message = string.IsNullOrEmpty(paramMessage.Value.ToString()) ? "Success" : paramMessage.Value.ToString();
                            response.Result = MapDataTableToBannerList(tblBanner);
                        }
                        else
                        {
                            response.StatusCode = 400;
                            response.Message = "Failed to delete banner.";
                            response.Result = null;
                        }
                    }
                    catch (MySqlException ex) when (ex.Number == 500)
                    {
                        //Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertAddToCartProduct : errormessage:" + ex.Message.ToString()));
                        response.StatusCode = 500;
                        response.Message = "ERROR : " + ex.Message;
                    }
                    catch (Exception ex)
                    {
                        //Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertAddToCartProduct : errormessage:" + ex.Message.ToString()));
                        response.StatusCode = 500;
                        response.Message = "ERROR : " + ex.Message;
                    }
                    return response;
                }
            }
        }

        public async Task<Response<Banner>> GetBanners()
        {
            var response = new Response<Banner>();
            using (MySqlConnection sqlcon = new MySqlConnection(ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(StoredProcedures.GET_BANNERS, sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        DataTable tblBanner = await Task.Run(() => _sqlDataHelper.SqlDataAdapterasync(cmd));

                        if (tblBanner.Rows.Count > 0)
                        {
                            response.StatusCode = 200;
                            response.Message = "Success";
                            response.Result = MapDataTableToBannerList(tblBanner);
                        }
                        else
                        {
                            response.StatusCode = 400;
                            response.Message = "Failed to fetch banners.";
                            response.Result = null;
                        }
                    }
                    catch (MySqlException ex) when (ex.Number == 500)
                    {
                        //Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertAddToCartProduct : errormessage:" + ex.Message.ToString()));
                        response.StatusCode = 500;
                        response.Message = "ERROR : " + ex.Message;
                    }
                    catch (Exception ex)
                    {
                        //Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertAddToCartProduct : errormessage:" + ex.Message.ToString()));
                        response.StatusCode = 500;
                        response.Message = "ERROR : " + ex.Message;
                    }
                    return response;
                }
            }
        }

        private static List<Banner> MapDataTableToBannerList(DataTable tblBanner)
        {
            List<Banner> lstBanner = new List<Banner>();
            foreach (DataRow banner in tblBanner.Rows)
            {
                Banner item = new Banner();
                item.BannerId = Convert.ToInt32(banner["BannerId"]);
                item.ImageUrl = banner["ImageUrl"].ToString() ?? "";
                item.BannerText = banner["BannerText"].ToString() ?? "";
                item.IsActive = Convert.ToInt32(banner["IsActive"]);                
                item.OrderSequence = Convert.ToInt32(banner["OrderSequence"]);                
                item.UploadedOn = banner["UploadedOn"] != DBNull.Value ? Convert.ToDateTime(banner["UploadedOn"]) : DateTime.MinValue;                

                lstBanner.Add(item);
            }
            return lstBanner;
        }
    }
}
