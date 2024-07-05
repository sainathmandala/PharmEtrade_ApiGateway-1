using BAL.ViewModel;
using DAL.Models;
using BAL.BusinessLogic.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using DAL;
using BAL.Common;
using BAL.ViewModels;

namespace BAL.BusinessLogic.Helper
{
    public class ProductHelper : IProductHelper
    {
        private readonly IsqlDataHelper _isqlDataHelper;
        private readonly string _connectionString;
        private readonly string _exPathToSave;

        public ProductHelper(IConfiguration configuration, IsqlDataHelper isqlDataHelper)
        {
            _isqlDataHelper = isqlDataHelper;
            _connectionString = configuration.GetConnectionString("OnlineexamDB");
            _exPathToSave = Path.Combine(Directory.GetCurrentDirectory(), "ProductExceptionLogs");
        }

        public async Task<string> InsertAddProduct(ProductFilter productviewmodel)
        {
            using (SqlConnection sqlcon = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertAddProduct", sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Productcategory_id", productviewmodel.Productcategory_id);
                    cmd.Parameters.AddWithValue("@ImageID", productviewmodel.ImageID);
                    cmd.Parameters.AddWithValue("@Sizeid", productviewmodel.Sizeid);
                    cmd.Parameters.AddWithValue("@ProductName", productviewmodel.ProductName);
                    cmd.Parameters.AddWithValue("@NDCorUPC", productviewmodel.NDCorUPC);
                    cmd.Parameters.AddWithValue("@BrandName", productviewmodel.BrandName);
                    cmd.Parameters.AddWithValue("@PriceName", productviewmodel.PriceName);
                    cmd.Parameters.AddWithValue("@UPNmemberPrice", productviewmodel.UPNmemberPrice);
                    cmd.Parameters.AddWithValue("@AmountInStock", productviewmodel.AmountInStock);
                    cmd.Parameters.AddWithValue("@Taxable", productviewmodel.Taxable);
                    cmd.Parameters.AddWithValue("@SalePrice", productviewmodel.SalePrice);
                    cmd.Parameters.AddWithValue("@SalePriceFrom", productviewmodel.SalePriceFrom);
                    cmd.Parameters.AddWithValue("@SalePriceTo", productviewmodel.SalePriceTo);
                    cmd.Parameters.AddWithValue("@Manufacturer", productviewmodel.Manufacturer);
                    cmd.Parameters.AddWithValue("@Strength", productviewmodel.Strength);
                    cmd.Parameters.AddWithValue("@Fromdate", productviewmodel.Fromdate);
                    cmd.Parameters.AddWithValue("@LotNumber", productviewmodel.LotNumber);
                    cmd.Parameters.AddWithValue("@ExpirationDate", productviewmodel.ExpirationDate);
                    cmd.Parameters.AddWithValue("@PackQuantity", productviewmodel.PackQuantity);
                    cmd.Parameters.AddWithValue("@PackType", productviewmodel.PackType);
                    cmd.Parameters.AddWithValue("@PackCondition", productviewmodel.PackCondition);
                    cmd.Parameters.AddWithValue("@ProductDescription", productviewmodel.ProductDescription);

                    try
                    {
                        await sqlcon.OpenAsync();
                        var result = await cmd.ExecuteNonQueryAsync();
                        return "Success"; // Return the number of affected rows
                    }
                    catch (Exception ex)
                    {
                        Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertProduct :  errormessage:" + ex.Message.ToString()));
                        // Handle the exception as needed
                        throw;
                    }
                }
            }
        }

        //public async Task<int> InsertAddToCartProduct(AddToCartViewModel addToCartModel)
        //{
        //    using (SqlConnection sqlcon = new SqlConnection(_connectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("InsertAddtoCartProduct", sqlcon))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            cmd.Parameters.AddWithValue("@Userid", addToCartModel.Userid);
        //            cmd.Parameters.AddWithValue("@Imageid", addToCartModel.Imageid);
        //            cmd.Parameters.AddWithValue("@ProductId", addToCartModel.ProductId);

        //            try
        //            {
        //                await sqlcon.OpenAsync();
        //                var result = await cmd.ExecuteScalarAsync();

        //                // Check if the product was successfully added
        //                if (result != null && int.TryParse(result.ToString(), out int newAddtoCartId))
        //                {
        //                    return newAddtoCartId; // Return the new AddtoCartId
        //                }
        //                else
        //                {
        //                    // Handle error or duplicate insert scenario
        //                    throw new Exception("Failed to add product to cart.");
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertAddToCartProduct : errormessage:" + ex.Message.ToString()));
        //                throw;
        //            }
        //        }
        //    }
        //}

        public async Task<string> InsertAddToCartProduct(AddToCartViewModel addToCartModel)
        {
            using (SqlConnection sqlcon = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertAddtoCartProduct", sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Userid", addToCartModel.Userid);
                    cmd.Parameters.AddWithValue("@Imageid", addToCartModel.Imageid);
                    cmd.Parameters.AddWithValue("@ProductId", addToCartModel.ProductId);

                    try
                    {
                        await sqlcon.OpenAsync();

                       
                        bool isProductAlreadyAdded = await IsProductAlreadyAdded(sqlcon, addToCartModel.Userid, addToCartModel.Imageid, addToCartModel.ProductId);

                        if (isProductAlreadyAdded)
                        {
                            throw new Exception("Product is already added to the cart.");
                        }

                        var result = await cmd.ExecuteScalarAsync();

                      
                        if (result != null && int.TryParse(result.ToString(), out int newAddtoCartId))
                        {
                            return "Success"; 
                        }
                        else
                        {
                           
                            throw new Exception("Failed to add product to cart.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertAddToCartProduct : errormessage:" + ex.Message.ToString()));
                        throw;
                    }
                }
            }
        }

        private async Task<bool> IsProductAlreadyAdded(SqlConnection sqlcon, int userId, int imageId, int productId)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM [PharmEtradeDB].[dbo].[AddtoCartproduct] WHERE Userid = @Userid AND Imageid = @Imageid AND ProductId = @ProductId", sqlcon))
            {
                cmd.Parameters.AddWithValue("@Userid", userId);
                cmd.Parameters.AddWithValue("@Imageid", imageId);
                cmd.Parameters.AddWithValue("@ProductId", productId);

                var count = await cmd.ExecuteScalarAsync();
                return (int)count > 0;
            }
        }

        public Task<Productviewmodel> DummyInterface(Productviewmodel pvm)
        {
            // Dummy implementation
            return Task.FromResult(pvm);
        }

        public async Task<IEnumerable<UserProductViewModel>> GetByUserId(int userId)
        {
            var products = new List<UserProductViewModel>();

            using (SqlConnection sqlcon = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetByUserId", sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    try
                    {
                        await sqlcon.OpenAsync();
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                products.Add(new UserProductViewModel
                                {
                                    ProductName = reader["ProductName"].ToString(),
                                    ImageUrl = reader["image_url"].ToString(),
                                    BrandName = reader["BrandName"].ToString(),
                                    PriceName = reader["PriceName"].ToString(),
                                    PackQuantity = Convert.ToInt32(reader["PackQuantity"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "GetByUserId : errormessage:" + ex.Message.ToString()));
                        throw;
                    }
                }
            }

            return products;
        }
        public async Task<string> SoftDeleteAddtoCartProduct(int addToCartId)
        {
            using (SqlConnection sqlcon = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SoftDeleteAddtoCartproduct", sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AddtoCartId", addToCartId);

                    try
                    {
                        await sqlcon.OpenAsync();
                        var result = await cmd.ExecuteScalarAsync();

                        if (result != null && result.ToString() == "AlreadyDeleted")
                        {
                            return "Failed"; 
                        }
                        else if (result != null && result.ToString() == "Success")
                        {
                            return "Success"; 
                        }
                        else
                        {
                            return "Failed"; 
                        }
                    }
                    catch (Exception ex)
                    {
                        Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "SoftDeleteAddtoCartProduct : errormessage:" + ex.Message.ToString()));
                        throw;
                    }
                }
            }
        }

        public async Task<string> InsertWishlistproduct(Wishlistviewmodel wishlistviewmodel)
        {
            using (SqlConnection sqlcon = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertWishlistItem", sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Userid", wishlistviewmodel.Userid);
                    cmd.Parameters.AddWithValue("@Imageid", wishlistviewmodel.Imageid);
                    cmd.Parameters.AddWithValue("@ProductId", wishlistviewmodel.ProductId);

                    try
                    {
                        await sqlcon.OpenAsync();

                        // Check if the product already exists for the user
                        bool isProductAlreadyAdded = await WishlistIsProductAlreadyAdded(sqlcon, wishlistviewmodel.Userid, wishlistviewmodel.Imageid, wishlistviewmodel.ProductId);

                        if (isProductAlreadyAdded)
                        {
                            throw new Exception("Product is already added to the wishlist.");
                        }

                        var result = await cmd.ExecuteScalarAsync();

                        // Check if the product was successfully added
                        if (result != null && int.TryParse(result.ToString(), out int newWishlistid))
                        {
                            return "Success"; 
                        }
                        else
                        {
                           
                            throw new Exception("Failed to add product to wishlist.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertWishlistproduct : errormessage:" + ex.Message.ToString()));
                        throw;
                    }
                }


            }

        }
        private async Task<bool> WishlistIsProductAlreadyAdded(SqlConnection sqlcon, int userId, int imageId, int productId)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM [PharmEtradeDB].[dbo].[Wishlist] WHERE Userid = @Userid AND Imageid = @Imageid AND ProductId = @ProductId", sqlcon))
            {
                cmd.Parameters.AddWithValue("@Userid", userId);
                cmd.Parameters.AddWithValue("@Imageid", imageId);
                cmd.Parameters.AddWithValue("@ProductId", productId);

                var count = await cmd.ExecuteScalarAsync();
                return (int)count > 0;
            }
        }
        public async Task<IEnumerable<UserProductViewModel>> GetwhislistByUserId(int userId)
        {
            var products = new List<UserProductViewModel>();

            using (SqlConnection sqlcon = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetWishlistByUserId", sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    try
                    {
                        await sqlcon.OpenAsync();
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                products.Add(new UserProductViewModel
                                {
                                    ProductName = reader["ProductName"].ToString(),
                                    ImageUrl = reader["image_url"].ToString(),
                                    BrandName = reader["BrandName"].ToString(),
                                    PriceName = reader["PriceName"].ToString(),
                                    PackQuantity = Convert.ToInt32(reader["PackQuantity"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "GetwhislistByUserId : errormessage:" + ex.Message.ToString()));
                        throw;
                    }
                }
            }

            return products;
        }
        public async Task<string> DeleteWishlistproduct(int wishlistid)
        {
            using (SqlConnection sqlcon = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SoftDeleteWishlistItem", sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@WishlistId", wishlistid);

                    try
                    {
                        await sqlcon.OpenAsync();
                        var result = await cmd.ExecuteScalarAsync();

                        if (result != null && result.ToString() == "AlreadyDeleted")
                        {
                            return "Failed";
                        }
                        else if (result != null && result.ToString() == "Success")
                        {
                            return "Success";
                        }
                        else
                        {
                            return "Failed";
                        }
                    }
                    catch (Exception ex)
                    {
                        Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "DeleteWishlistproduct : errormessage:" + ex.Message.ToString()));
                        throw;
                    }
                }
            }

        }



    }
}
