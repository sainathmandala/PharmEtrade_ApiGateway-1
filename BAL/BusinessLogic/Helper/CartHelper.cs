using BAL.BusinessLogic.Interface;
using BAL.Models;
using BAL.ResponseModels;
using DAL;
using MySql.Data.MySqlClient;
using System.Data;

namespace BAL.BusinessLogic.Helper
{
    public class CartHelper : ICartHelper
    {
        private IsqlDataHelper _sqlDataHelper;
        public CartHelper(IsqlDataHelper isqlDataHelper)
        {
            _sqlDataHelper = isqlDataHelper;
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

                List<Cart> lstCart = new List<Cart>();
                foreach (DataRow cartItem in tblCart.Rows)
                {
                    Cart item = new Cart();
                    item.CartId = cartItem["CartId"].ToString();                    
                    item.Quantity = Convert.ToInt32(cartItem["Quantity"]);
                    item.IsActive = Convert.ToInt32(cartItem["IsActive"]) == 0 ? true : false;                    
                    item.AddedOn = cartItem["AddedOn"] != DBNull.Value ? Convert.ToDateTime(cartItem["AddedOn"]) : DateTime.MinValue;                    

                    //Add Basic Customer Details
                    item.Customer.CustomerId = cartItem["CustomerId"].ToString();
                    item.Customer.FirstName = cartItem["FirstName"].ToString();
                    item.Customer.LastName = cartItem["LastName"].ToString();
                    item.Customer.Email = cartItem["Email"].ToString();
                    item.Customer.Mobile = cartItem["Mobile"].ToString();
                    item.Customer.CustomerTypeId = Convert.ToInt32(cartItem["CustomerTypeId"]);
                    item.Customer.AccountTypeId = Convert.ToInt32(cartItem["AccountTypeId"]);
                    item.Customer.IsUPNMember = Convert.ToInt32(cartItem["IsUPNMember"]);

                    //Add Basic Product Details
                    item.Product.ProductID = cartItem["ProductID"].ToString();
                    item.Product.ProductCategoryId = Convert.ToInt32(cartItem["ProductCategoryId"]);
                    item.Product.ProductGalleryId = Convert.ToInt32(cartItem["ProductGalleryId"]);
                    item.Product.ProductName = cartItem["ProductName"].ToString();
                    item.Product.SalePrice = Convert.ToDecimal(cartItem["SalePrice"]);
                    item.Product.BrandName = cartItem["BrandName"].ToString();
                    item.Product.Manufacturer = cartItem["Manufacturer"].ToString();
                    item.Product.UriKey = cartItem["UriKey"].ToString();
                    item.Product.ImageUrl = cartItem["ImageUrl"].ToString();
                    item.Product.Caption = cartItem["Caption"].ToString();

                    lstCart.Add(item);
                }
                response.StatusCode = 200;
                response.Message = "Successfully Feched data.";
                response.Result = lstCart;
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
