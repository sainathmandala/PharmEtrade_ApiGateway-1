using BAL.BusinessLogic.Interface;
using BAL.Models;
using BAL.RequestModels;
using DAL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using BAL.ResponseModels;
using BAL.ViewModels;


namespace BAL.BusinessLogic.Helper
{
    public  class WishListHelper: IWishListHelper
    {
        private IsqlDataHelper _sqlDataHelper;
        public WishListHelper(IsqlDataHelper isqlDataHelper)
        {
            _sqlDataHelper = isqlDataHelper;
        }

        public async Task<Response<WishList>> AddToWishList(WishListRequest request)
        {
            var response = new Response<WishList>();
            try
            {
                MySqlCommand command = new MySqlCommand("sp_AddUpdateWishlist");
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("p_WishListId", request.WishListId);
                command.Parameters.AddWithValue("p_CustomerId", request.CustomerId);
                command.Parameters.AddWithValue("p_ProductId", request.ProductId);
                command.Parameters.AddWithValue("p_IsActive", 1);
                DataTable tblwishlist = await Task.Run(() => _sqlDataHelper.SqlDataAdapterasync(command));

                response.StatusCode = 200;
                response.Message = "Successfully Feched data.";
                response.Result = MapDataTableToWishListList(tblwishlist); 
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.Result = null;
            }
            return response;
        }

        private static List<WishList> MapDataTableToWishListList(DataTable tblwishlist)
        {
            List<WishList> lstwishlist = new List<WishList>();
            foreach (DataRow wishlistItem in tblwishlist.Rows)
            {
                WishList item = new WishList();
                item.WishListId = wishlistItem["WishListId"].ToString();
                item.IsActive = Convert.ToInt32(wishlistItem["IsActive"]) == 0 ? true : false;
                item.DeletedOn = wishlistItem["DeletedOn"] != DBNull.Value ? Convert.ToDateTime(wishlistItem["DeletedOn"]) : DateTime.MinValue;

                //Add Basic Customer Details
                item.Customer = new CustomerBasicDetails();
                item.Product = new ProductBasicDetails();
                //string str = wishlistItem["CustomerId"].ToString();
                item.Customer.CustomerId = wishlistItem["CustomerId"].ToString();
                item.Customer.FirstName = wishlistItem["FirstName"].ToString();
                item.Customer.LastName = wishlistItem["LastName"].ToString();
                item.Customer.Email = wishlistItem["Email"].ToString();
                item.Customer.Mobile = wishlistItem["Mobile"].ToString();
                item.Customer.CustomerTypeId = Convert.ToInt32(wishlistItem["CustomerTypeId"]);
                item.Customer.AccountTypeId = Convert.ToInt32(wishlistItem["AccountTypeId"]);
                item.Customer.IsUPNMember = Convert.ToInt32(wishlistItem["IsUPNMember"]);

                //Add Basic Product Details
                item.Product.ProductID = wishlistItem["ProductID"].ToString();
                item.Product.ProductCategoryId = Convert.ToInt32(wishlistItem["ProductCategoryId"]);
                item.Product.ProductGalleryId = Convert.ToInt32(wishlistItem["ProductGalleryId"]);
                item.Product.ProductName = wishlistItem["ProductName"].ToString();
                item.Product.SalePrice = Convert.ToDecimal(wishlistItem["SalePrice"]);
                item.Product.BrandName = wishlistItem["BrandName"].ToString();
                item.Product.Manufacturer = wishlistItem["Manufacturer"].ToString();
                item.Product.UriKey = wishlistItem["UriKey"].ToString();
                item.Product.ImageUrl = wishlistItem["ImageUrl"].ToString();
                item.Product.Caption = wishlistItem["Caption"].ToString();

                lstwishlist.Add(item);
            }
            return lstwishlist;
        }

        public async  Task<Response<WishList>> GetWishListItems(string customerId = null)
        {
            var response = new Response<WishList>();
            try
            {
                MySqlCommand command = new MySqlCommand("sp_GetWishListItems");
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("p_CustomerId", customerId);
                DataTable tblwishlist = await Task.Run(() => _sqlDataHelper.SqlDataAdapterasync(command));

                response.StatusCode = 200;
                response.Message = "Successfully Feched data.";
                response.Result = MapDataTableToWishListList(tblwishlist);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.Result = null;
            }
            return response;
        }
        public async Task<Response<WishList>> GetWishListById(string WishListId = null)
        {
            var response = new Response<WishList>();
            try
            {
                MySqlCommand command = new MySqlCommand("sp_GetWishListById");
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("p_WishListId", WishListId);
                DataTable tblwishlist = await Task.Run(() => _sqlDataHelper.SqlDataAdapterasync(command));

                response.StatusCode = 200;
                response.Message = "Successfully Feched data.";
                response.Result = MapDataTableToWishListList(tblwishlist);
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
