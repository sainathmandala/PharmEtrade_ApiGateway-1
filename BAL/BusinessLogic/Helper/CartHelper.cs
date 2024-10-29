using BAL.BusinessLogic.Interface;
using BAL.Common;
using BAL.Models;
using BAL.RequestModels;
using BAL.ResponseModels;
using BAL.ViewModels;
using DAL;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;
using Microsoft.Extensions.Configuration;



namespace BAL.BusinessLogic.Helper
{
    public class CartHelper : ICartHelper
    {
        private IConfiguration _configuration;

        private IsqlDataHelper _sqlDataHelper;
        public CartHelper(IsqlDataHelper isqlDataHelper)
        {
            _sqlDataHelper = isqlDataHelper;
        }
        private string ConnectionString
        {
            get
            {
                return _configuration.GetConnectionString("APIDBConnectionString") ?? "";
            }
        }

        public async Task<Response<Cart>> AddToCart(CartRequest request)
        {
            var response = new Response<Cart>();
            try
            {
                MySqlCommand command = new MySqlCommand("sp_AddUpdateCart");
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("p_CartId", request.CartId);
                command.Parameters.AddWithValue("p_CustomerId", request.CustomerId);
                command.Parameters.AddWithValue("p_ProductId", request.ProductId);
                command.Parameters.AddWithValue("p_Quantity", request.Quantity);
                command.Parameters.AddWithValue("p_IsActive", 1);
                DataTable tblCart = await Task.Run(() => _sqlDataHelper.SqlDataAdapterasync(command));

                response.StatusCode = 200;
                response.Message = "Successfully Feched data.";
                response.Result = MapDataTableToCartList(tblCart); ;
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.Result = null;
            }
            return response;
        }

        public async Task<Response<Cart>> GetCartItems(string customerId = null, string productId = null)
        {
            var response = new Response<Cart>();
            try
            {
                MySqlCommand command = new MySqlCommand("sp_GetCartItems");
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("p_CustomerId", customerId);
                command.Parameters.AddWithValue("P_ProductId", productId);
                DataTable tblCart = await Task.Run(() => _sqlDataHelper.SqlDataAdapterasync(command));

                response.StatusCode = 200;
                response.Message = "Successfully Feched data.";
                response.Result = MapDataTableToCartList(tblCart);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.Result = null;
            }
            return response;
        }

        private static List<Cart> MapDataTableToCartList(DataTable tblCart)
        {
            List<Cart> lstCart = new List<Cart>();
            foreach (DataRow cartItem in tblCart.Rows)
            {
                Cart item = new Cart();
                item.CartId = cartItem["CartId"].ToString();
                item.Quantity = Convert.ToInt32(cartItem["Quantity"] != DBNull.Value ? cartItem["Quantity"] : 0);
                item.IsActive = Convert.ToInt32(cartItem["IsActive"]) == 1 ? true : false;
                item.AddedOn = cartItem["AddedOn"] != DBNull.Value ? Convert.ToDateTime(cartItem["AddedOn"]) : DateTime.MinValue;

                //Add Basic Customer Details
                item.Customer.CustomerId = cartItem["CustomerId"].ToString();
                item.Customer.FirstName = cartItem["FirstName"].ToString();
                item.Customer.LastName = cartItem["LastName"].ToString();
                item.Customer.Email = cartItem["Email"].ToString();
                item.Customer.Mobile = cartItem["Mobile"].ToString();
                item.Customer.CustomerTypeId = Convert.ToInt32(cartItem["CustomerTypeId"] != DBNull.Value ? cartItem["CustomerTypeId"] : 0);
                item.Customer.AccountTypeId = Convert.ToInt32(cartItem["AccountTypeId"] != DBNull.Value ? cartItem["AccountTypeId"] : 0);
                item.Customer.IsUPNMember = Convert.ToInt32(cartItem["IsUPNMember"] != DBNull.Value ? cartItem["IsUPNMember"] : 0);

                //Add Basic Product Details
                item.Product.ProductID = cartItem["ProductID"].ToString() ?? "";
                item.Product.ProductCategoryId = Convert.ToInt32(cartItem["ProductCategoryId"] != DBNull.Value ? cartItem["ProductCategoryId"] : 0);
                item.Product.ProductGalleryId = cartItem["ProductGalleryId"].ToString() ?? "";
                item.Product.ProductName = cartItem["ProductName"].ToString() ?? "";
                item.Product.SalePrice = Convert.ToDecimal(cartItem["SalePrice"] != DBNull.Value ? cartItem["SalePrice"] : 0.0);
                item.Product.BrandName = cartItem["BrandName"].ToString() ?? "";
                item.Product.Manufacturer = cartItem["Manufacturer"].ToString() ?? "";
                item.Product.UriKey = cartItem["UriKey"].ToString() ?? "";
                item.Product.ImageUrl = cartItem["ImageUrl"].ToString() ?? "";
                item.Product.Caption = cartItem["Caption"].ToString() ?? "";
                item.Product.UnitPrice = Convert.ToDecimal(cartItem["UnitPrice"] != DBNull.Value ? cartItem["UnitPrice"] : 0.0);
                item.Product.MaximumOrderQuantity = Convert.ToInt32(cartItem["MaximumQuantity"] != DBNull.Value ? cartItem["MaximumQuantity"] : 0);
                item.Product.MinimumOrderQuantity = Convert.ToInt32(cartItem["MinimumQuantity"] != DBNull.Value ? cartItem["MinimumQuantity"] : 0);
                item.Product.AmountInStock = Convert.ToInt32(cartItem["AmountInStock"] != DBNull.Value ? cartItem["AmountInStock"] : 0);
                item.Product.SellerId = cartItem["SellerId"].ToString() ?? "";
                item.Product.SellerName = cartItem["SellerName"].ToString() ?? "";

                lstCart.Add(item);
            }
            return lstCart;
        }

        public async Task<Response<Cart>> DeleteCart(string CartId)
        {
            var response = new Response<Cart>();
            using (MySqlConnection sqlcon = new MySqlConnection(ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(StoredProcedures.DELETE_CART, sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_CartId", CartId);
                    MySqlParameter paramMessage = new MySqlParameter("@o_Message", MySqlDbType.String)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(paramMessage);
                    try
                    {
                        DataTable tblcart = await Task.Run(() => _sqlDataHelper.SqlDataAdapterasync(cmd));
                        if (tblcart.Rows.Count > 0)
                        {
                            response.StatusCode = 200;
                            response.Message = string.IsNullOrEmpty(paramMessage.Value.ToString()) ? "Success" : paramMessage.Value.ToString();
                            response.Result = MapDataTableToCartList(tblcart);
                        }
                        else
                        {
                            response.StatusCode = 400;
                            response.Message = "Failed to Delete Cart";
                            response.Result = null;
                        }
                    }
                    catch (MySqlException ex) when (ex.Number == 500)
                    {
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
    }
}
