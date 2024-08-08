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
using System.Net.Http.Headers;
using OfficeOpenXml;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using MySql.Data.MySqlClient;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Drawing.Imaging;
using System.Security.AccessControl;
namespace BAL.BusinessLogic.Helper
{
    public class ProductHelper : IProductHelper
    {
        private readonly IsqlDataHelper _isqlDataHelper;
        private readonly string _connectionString;
        private readonly string _exPathToSave;
        private readonly IConfiguration _configuration;
        private readonly S3Helper _s3Helper;
        public ProductHelper(IConfiguration configuration, IsqlDataHelper isqlDataHelper)
        {
            _s3Helper = new S3Helper(configuration);
            _configuration = configuration;
            _isqlDataHelper = isqlDataHelper;
            _connectionString = configuration.GetConnectionString("OnlineexamDB");
            _exPathToSave = Path.Combine(Directory.GetCurrentDirectory(), "ProductExceptionLogs");
        }

        // Author: [swathi]
        // Created Date: [02/07/2024]
        // Description: Method for InsertProducts
        //public async Task<string> InsertAddProduct(ProductFilter productviewmodel, Stream imageFileStream, string imageFileName)
        // {
        //    using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
        //    {
        //        try
        //        {
        //            await sqlcon.OpenAsync();

        //            // Begin a transaction
        //            MySqlTransaction transaction = await sqlcon.BeginTransactionAsync();

        //            // Step 1: Define the folder name
        //            string folderName = "PharmaEtrade";

        //            // Step 2: Upload Image to AWS S3
        //            string imageUrl = await _s3Helper.UploadFileAsync(imageFileStream, folderName, imageFileName);

        //            // Step 3: Insert Image URL and get ImageID
        //            using (MySqlCommand cmdImage = new MySqlCommand("InsertImageUrl", sqlcon, transaction))
        //            {
        //                cmdImage.CommandType = CommandType.StoredProcedure;
        //                cmdImage.Parameters.AddWithValue("@p_ImageUrl", imageUrl);
        //                cmdImage.Parameters.AddWithValue("@p_Caption", productviewmodel.Caption);
        //                MySqlParameter imageIDParam = new MySqlParameter("@p_ImageID", MySqlDbType.Int32)
        //                {
        //                    Direction = ParameterDirection.Output
        //                };
        //                cmdImage.Parameters.Add(imageIDParam);

        //                await cmdImage.ExecuteNonQueryAsync();
        //                int imageID = (int)imageIDParam.Value;

        //                // Step 4: Insert Product using the obtained ImageID
        //                using (MySqlCommand cmdProduct = new MySqlCommand("InsertAddProduct", sqlcon, transaction))
        //                {
        //                    cmdProduct.CommandType = CommandType.StoredProcedure;

        //                    cmdProduct.Parameters.AddWithValue("@p_Productcategory_id", productviewmodel.Productcategory_id);
        //                    cmdProduct.Parameters.AddWithValue("@p_ImageID", imageID);
        //                    cmdProduct.Parameters.AddWithValue("@p_Sizeid", productviewmodel.Sizeid);
        //                    cmdProduct.Parameters.AddWithValue("@p_ProductName", productviewmodel.ProductName);
        //                    cmdProduct.Parameters.AddWithValue("@p_NDCorUPC", productviewmodel.NDCorUPC);
        //                    cmdProduct.Parameters.AddWithValue("@p_BrandName", productviewmodel.BrandName);
        //                    cmdProduct.Parameters.AddWithValue("@p_PriceName", productviewmodel.PriceName);
        //                    cmdProduct.Parameters.AddWithValue("@p_UPNmemberPrice", productviewmodel.UPNmemberPrice);
        //                    cmdProduct.Parameters.AddWithValue("@p_AmountInStock", productviewmodel.AmountInStock);
        //                    cmdProduct.Parameters.AddWithValue("@p_Taxable", productviewmodel.Taxable);
        //                    cmdProduct.Parameters.AddWithValue("@p_SalePrice", productviewmodel.SalePrice);
        //                    cmdProduct.Parameters.AddWithValue("@p_SalePriceFrom", productviewmodel.SalePriceFrom);
        //                    cmdProduct.Parameters.AddWithValue("@p_SalePriceTo", productviewmodel.SalePriceTo);
        //                    cmdProduct.Parameters.AddWithValue("@p_Manufacturer", productviewmodel.Manufacturer);
        //                    cmdProduct.Parameters.AddWithValue("@p_Strength", productviewmodel.Strength);
        //                    cmdProduct.Parameters.AddWithValue("@p_Fromdate", productviewmodel.Fromdate);
        //                    cmdProduct.Parameters.AddWithValue("@p_LotNumber", productviewmodel.LotNumber);
        //                    cmdProduct.Parameters.AddWithValue("@p_ExpirationDate", productviewmodel.ExpirationDate);
        //                    cmdProduct.Parameters.AddWithValue("@p_PackQuantity", productviewmodel.PackQuantity);
        //                    cmdProduct.Parameters.AddWithValue("@p_PackType", productviewmodel.PackType);
        //                    cmdProduct.Parameters.AddWithValue("@p_PackCondition", productviewmodel.PackCondition);
        //                    cmdProduct.Parameters.AddWithValue("@p_ProductDescription", productviewmodel.ProductDescription);
        //                    cmdProduct.Parameters.AddWithValue("@p_MetaKeywords", productviewmodel.MetaKeywords);
        //                    cmdProduct.Parameters.AddWithValue("@p_MetaTitle", productviewmodel.MetaTitle);
        //                    cmdProduct.Parameters.AddWithValue("@p_MetaDescription", productviewmodel.MetaDescription);
        //                    cmdProduct.Parameters.AddWithValue("@p_SaltComposition", productviewmodel.SaltComposition);
        //                    cmdProduct.Parameters.AddWithValue("@p_UriKey", productviewmodel.UriKey);
        //                    cmdProduct.Parameters.AddWithValue("@p_AboutTheProduct", productviewmodel.AboutTheProduct);
        //                    cmdProduct.Parameters.AddWithValue("@p_CategorySpecificationId", productviewmodel.CategorySpecificationId);
        //                    cmdProduct.Parameters.AddWithValue("@p_ProductTypeId", productviewmodel.ProductTypeId);
        //                    cmdProduct.Parameters.AddWithValue("@p_SellerId", productviewmodel.SellerId);


        //                    await cmdProduct.ExecuteNonQueryAsync();
        //                }

        //                // Commit the transaction
        //                await transaction.CommitAsync();

        //                return "Success";
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertProduct :  errormessage:" + ex.Message.ToString()));
        //            // Handle the exception as needed
        //            throw;
        //        }
        //    }
        //}

        public async Task<string> InsertAddProduct(ProductFilter productviewmodel, Stream imageFileStream, string imageFileName)
        {
            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                try
                {
                    await sqlcon.OpenAsync();

                    // Begin a transaction
                    MySqlTransaction transaction = await sqlcon.BeginTransactionAsync();

                    // Step 1: Define the folder name
                    string folderName = "PharmaEtrade";

                    // Step 2: Upload Image to AWS S3
                    string imageUrl = await _s3Helper.UploadFileAsync(imageFileStream, folderName, imageFileName);

                    // Step 3: Insert Image URL and get ImageID
                    using (MySqlCommand cmdImage = new MySqlCommand("InsertImageUrl", sqlcon, transaction))
                    {
                        cmdImage.CommandType = CommandType.StoredProcedure;
                        cmdImage.Parameters.AddWithValue("@p_ImageUrl", imageUrl);
                        cmdImage.Parameters.AddWithValue("@p_Caption", productviewmodel.Caption);
                        MySqlParameter imageIDParam = new MySqlParameter("@p_ImageID", MySqlDbType.Int32)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmdImage.Parameters.Add(imageIDParam);

                        await cmdImage.ExecuteNonQueryAsync();
                        int imageID = (int)imageIDParam.Value;

                        // Step 4: Insert Product using the obtained ImageID
                        using (MySqlCommand cmdProduct = new MySqlCommand("InsertAddProduct", sqlcon, transaction))
                        {
                            cmdProduct.CommandType = CommandType.StoredProcedure;

                            cmdProduct.Parameters.AddWithValue("@p_Productcategory_id", productviewmodel.Productcategory_id);
                            cmdProduct.Parameters.AddWithValue("@p_ImageID", imageID);
                            cmdProduct.Parameters.AddWithValue("@p_Sizeid", productviewmodel.Sizeid);
                            cmdProduct.Parameters.AddWithValue("@p_ProductName", productviewmodel.ProductName);
                            cmdProduct.Parameters.AddWithValue("@p_NDCorUPC", productviewmodel.NDCorUPC);
                            cmdProduct.Parameters.AddWithValue("@p_BrandName", productviewmodel.BrandName);
                            cmdProduct.Parameters.AddWithValue("@p_PriceName", productviewmodel.PriceName);
                            cmdProduct.Parameters.AddWithValue("@p_UPNmemberPrice", productviewmodel.UPNmemberPrice);
                            cmdProduct.Parameters.AddWithValue("@p_AmountInStock", productviewmodel.AmountInStock);
                            cmdProduct.Parameters.AddWithValue("@p_Taxable", productviewmodel.Taxable);
                            cmdProduct.Parameters.AddWithValue("@p_SalePrice", productviewmodel.SalePrice);
                            cmdProduct.Parameters.AddWithValue("@p_SalePriceFrom", productviewmodel.SalePriceFrom);
                            cmdProduct.Parameters.AddWithValue("@p_SalePriceTo", productviewmodel.SalePriceTo);
                            cmdProduct.Parameters.AddWithValue("@p_Manufacturer", productviewmodel.Manufacturer);
                            cmdProduct.Parameters.AddWithValue("@p_Strength", productviewmodel.Strength);
                            cmdProduct.Parameters.AddWithValue("@p_Fromdate", productviewmodel.Fromdate);
                            cmdProduct.Parameters.AddWithValue("@p_LotNumber", productviewmodel.LotNumber);
                            cmdProduct.Parameters.AddWithValue("@p_ExpirationDate", productviewmodel.ExpirationDate);
                            cmdProduct.Parameters.AddWithValue("@p_PackQuantity", productviewmodel.PackQuantity);
                            cmdProduct.Parameters.AddWithValue("@p_PackType", productviewmodel.PackType);
                            cmdProduct.Parameters.AddWithValue("@p_PackCondition", productviewmodel.PackCondition);
                            cmdProduct.Parameters.AddWithValue("@p_ProductDescription", productviewmodel.ProductDescription);
                            cmdProduct.Parameters.AddWithValue("@p_MetaKeywords", productviewmodel.MetaKeywords);
                            cmdProduct.Parameters.AddWithValue("@p_MetaTitle", productviewmodel.MetaTitle);
                            cmdProduct.Parameters.AddWithValue("@p_MetaDescription", productviewmodel.MetaDescription);
                            cmdProduct.Parameters.AddWithValue("@p_SaltComposition", productviewmodel.SaltComposition);
                            cmdProduct.Parameters.AddWithValue("@p_UriKey", productviewmodel.UriKey);
                            cmdProduct.Parameters.AddWithValue("@p_AboutTheProduct", productviewmodel.AboutTheProduct);
                            cmdProduct.Parameters.AddWithValue("@p_CategorySpecificationId", productviewmodel.CategorySpecificationId);
                            cmdProduct.Parameters.AddWithValue("@p_ProductTypeId", productviewmodel.ProductTypeId);
                            cmdProduct.Parameters.AddWithValue("@p_SellerId", productviewmodel.SellerId);

                            await cmdProduct.ExecuteNonQueryAsync();
                        }

                        // Commit the transaction
                        await transaction.CommitAsync();

                        return "Success";
                    }
                }
                catch (Exception ex)
                {
                    Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertProduct :  errormessage:" + ex.Message.ToString()));
                    // Handle the exception as needed
                    throw;
                }
            }
        }

        // Author: [swathi]
        // Created Date: [10/07/2024]
        // Description: Method for BulkInsertProducts
        public async Task<string> ProcessExcelFileAsync(IFormFile file)
        {
            var products = new List<ProductFilter>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial; 
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++) 
                    {
                        var product = new ProductFilter
                        {
                            Productcategory_id = int.Parse(worksheet.Cells[row, 1].Text),
                           // ImageID = int.Parse(worksheet.Cells[row, 2].Text),
                            Sizeid = int.Parse(worksheet.Cells[row, 3].Text),
                            ProductName = worksheet.Cells[row, 4].Text,
                            NDCorUPC = worksheet.Cells[row, 5].Text,
                            BrandName = worksheet.Cells[row, 6].Text,
                            PriceName = decimal.Parse(worksheet.Cells[row, 7].Text),
                            UPNmemberPrice = decimal.Parse(worksheet.Cells[row, 8].Text),
                            AmountInStock = int.Parse(worksheet.Cells[row, 9].Text),
                            Taxable = bool.Parse(worksheet.Cells[row, 10].Text),
                            SalePrice = decimal.Parse(worksheet.Cells[row, 11].Text),
                            SalePriceFrom = DateTime.Parse(worksheet.Cells[row, 12].Text),
                            SalePriceTo = DateTime.Parse(worksheet.Cells[row, 13].Text),
                            Manufacturer = worksheet.Cells[row, 14].Text,
                            Strength = worksheet.Cells[row, 15].Text,
                            Fromdate = DateTime.Parse(worksheet.Cells[row, 16].Text),
                            LotNumber = worksheet.Cells[row, 17].Text,
                            ExpirationDate = DateTime.Parse(worksheet.Cells[row, 18].Text),
                            PackQuantity = int.Parse(worksheet.Cells[row, 19].Text),
                            PackType = worksheet.Cells[row, 20].Text,
                            PackCondition = worksheet.Cells[row, 21].Text,
                            ProductDescription = worksheet.Cells[row, 22].Text,
                           // ImageUrl = worksheet.Cells[row,23].Text,
                            Caption = worksheet.Cells[row,24].Text
                        };
                        
                        products.Add(product);
                    }
                }
            }

            foreach (var product in products)
            {
                await InsertAddProduct(product, Stream.Null, string.Empty);
            }

            return "Success";
        }




        // Author: [Mamatha]
        // Created Date: [04/07/2024]
        // Description: Method for EditProductDetails
        public async Task<string> EditProductDetails(int AddproductID, ProductFilter productviewmodel, Stream imageFileStream, string imageFileName)
        {
            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                MySqlTransaction transaction = null;
                try
                {
                    await sqlcon.OpenAsync();
                    transaction = await sqlcon.BeginTransactionAsync();

                    // Get existing ImageID for the product
                    int imageID = 0;
                    using (MySqlCommand cmdGetImageID = new MySqlCommand("SELECT ImageID FROM addproduct WHERE AddproductID = @AddproductID", sqlcon, transaction))
                    {
                        cmdGetImageID.Parameters.AddWithValue("@AddproductID", AddproductID);
                        var result = await cmdGetImageID.ExecuteScalarAsync();
                        if (result != null && result != DBNull.Value)
                        {
                            imageID = Convert.ToInt32(result);
                        }
                    }

                    // If there's an image to upload, update the existing image details
                    if (imageFileStream != Stream.Null && !string.IsNullOrEmpty(imageFileName))
                    {
                        string folderName = "PharmaEtrade";
                        string imageUrl = await _s3Helper.UploadFileAsync(imageFileStream, folderName, imageFileName);

                        using (MySqlCommand cmdImage = new MySqlCommand("UpdateImageUrl", sqlcon, transaction))
                        {
                            cmdImage.CommandType = CommandType.StoredProcedure;
                            cmdImage.Parameters.AddWithValue("@p_ImageID", imageID);
                            cmdImage.Parameters.AddWithValue("@p_ImageUrl", imageUrl);
                            cmdImage.Parameters.AddWithValue("@p_Caption", productviewmodel.Caption);

                            await cmdImage.ExecuteNonQueryAsync();
                        }
                    }

                    // Execute the stored procedure to update the product details
                    using (MySqlCommand cmdProduct = new MySqlCommand("SP_EditProductDetails", sqlcon, transaction))
                    {
                        cmdProduct.CommandType = CommandType.StoredProcedure;

                        cmdProduct.Parameters.AddWithValue("p_AddproductID", productviewmodel.AddproductID);
                        cmdProduct.Parameters.AddWithValue("p_Productcategory_id", productviewmodel.Productcategory_id);
                        cmdProduct.Parameters.AddWithValue("p_ImageID", imageID); // Use the fetched imageID
                        cmdProduct.Parameters.AddWithValue("p_Sizeid", productviewmodel.Sizeid);
                        cmdProduct.Parameters.AddWithValue("p_ProductName", productviewmodel.ProductName);
                        cmdProduct.Parameters.AddWithValue("p_NDCorUPC", productviewmodel.NDCorUPC);
                        cmdProduct.Parameters.AddWithValue("p_BrandName", productviewmodel.BrandName);
                        cmdProduct.Parameters.AddWithValue("p_PriceName", productviewmodel.PriceName);
                        cmdProduct.Parameters.AddWithValue("p_UPNmemberPrice", productviewmodel.UPNmemberPrice);
                        cmdProduct.Parameters.AddWithValue("p_AmountInStock", productviewmodel.AmountInStock);
                        cmdProduct.Parameters.AddWithValue("p_Taxable", productviewmodel.Taxable);
                        cmdProduct.Parameters.AddWithValue("p_SalePrice", productviewmodel.SalePrice);
                        cmdProduct.Parameters.AddWithValue("p_SalePriceFrom",productviewmodel.SalePriceFrom);
                        cmdProduct.Parameters.AddWithValue("p_SalePriceTo", productviewmodel.SalePriceTo);
                        cmdProduct.Parameters.AddWithValue("p_Manufacturer", productviewmodel.Manufacturer);
                        cmdProduct.Parameters.AddWithValue("p_Strength", productviewmodel.Strength);
                        cmdProduct.Parameters.AddWithValue("p_Fromdate", productviewmodel.Fromdate);
                        cmdProduct.Parameters.AddWithValue("p_LotNumber",productviewmodel.LotNumber);
                        cmdProduct.Parameters.AddWithValue("p_ExpirationDate",productviewmodel.ExpirationDate);
                        cmdProduct.Parameters.AddWithValue("p_PackQuantity",productviewmodel.PackQuantity);
                        cmdProduct.Parameters.AddWithValue("p_PackType", productviewmodel.PackType);
                        cmdProduct.Parameters.AddWithValue("p_PackCondition", productviewmodel.PackCondition);
                        cmdProduct.Parameters.AddWithValue("p_ProductDescription", productviewmodel.ProductDescription);
                        cmdProduct.Parameters.AddWithValue("p_MetaKeywords",productviewmodel.MetaKeywords);
                        cmdProduct.Parameters.AddWithValue("p_MetaTitle", productviewmodel.MetaTitle);
                        cmdProduct.Parameters.AddWithValue("p_MetaDescription",productviewmodel.MetaDescription);
                        cmdProduct.Parameters.AddWithValue("p_SaltComposition", productviewmodel.SaltComposition);
                        cmdProduct.Parameters.AddWithValue("p_UriKey", productviewmodel.UriKey);
                        cmdProduct.Parameters.AddWithValue("p_AboutTheProduct", productviewmodel.AboutTheProduct);
                        cmdProduct.Parameters.AddWithValue("p_CategorySpecificationId", productviewmodel.CategorySpecificationId);
                        cmdProduct.Parameters.AddWithValue("p_ProductTypeId", productviewmodel.ProductTypeId);
                        cmdProduct.Parameters.AddWithValue("p_SellerId", productviewmodel.SellerId);

                        await cmdProduct.ExecuteNonQueryAsync();
                    }

                    // Commit the transaction
                    await transaction.CommitAsync();
                    return "Success";

                }
                catch (Exception ex)
                {
                    Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "EditProductDetails:ErrorMessage-" + ex.Message.ToString()));
                    // Rollback transaction in case of error
                    if (transaction != null)
                    {
                        await transaction.RollbackAsync();
                    }
                    throw;
                }
            }
        }



        // Author: [swathi]
        // Created Date: [02/07/2024]
        // Description: Method for AddtoCartProducts

        public async Task<string> InsertAddToCartProduct(AddToCartViewModel addToCartModel)
        {
            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("InsertAddtoCartProduct", sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_Userid", addToCartModel.Userid);
                    cmd.Parameters.AddWithValue("@p_Imageid", addToCartModel.Imageid);
                    cmd.Parameters.AddWithValue("@p_ProductId", addToCartModel.ProductId);
                    cmd.Parameters.AddWithValue("@p_Quantity", addToCartModel.Quantity);

                    MySqlParameter newCartIdParam = new MySqlParameter("@p_NewCartId", MySqlDbType.Int32);
                    newCartIdParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(newCartIdParam);

                    try
                    {
                        await sqlcon.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        if (newCartIdParam.Value != null && int.TryParse(newCartIdParam.Value.ToString(), out int newAddtoCartId))
                        {
                            return "Success";
                        }
                        else
                        {
                            throw new Exception("Failed to add product to cart.");
                        }
                    }
                    catch (MySqlException ex) when (ex.Number == 500)
                    {
                        Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertAddToCartProduct : errormessage:" + ex.Message.ToString()));
                        return "Error: " + ex.Message;
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
        // Author: [swathi]
        // Created Date: [03/07/2024]
        // Description: Method for GetCartProducts based on userid
        public async Task<IEnumerable<UserProductViewModel>> GetCartByUserId(int userId)
        {
            var products = new List<UserProductViewModel>();

            using (var sqlcon = new MySqlConnection(_connectionString))
            {
                using (var cmd = new MySqlCommand("GetCartByUserId", sqlcon))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_UserId", userId);

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
        // Author: [swathi]
        // Created Date: [04/07/2024]
        // Description: Method for  Delete CartProduct
        public async Task<string> SoftDeleteAddtoCartProduct(int addToCartId)
        {
            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("SoftDeleteAddtoCartproduct", sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_AddtoCartId", addToCartId);

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
                    catch (MySqlException ex)
                    {
                        Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "SoftDeleteAddtoCartProduct : errormessage:" + ex.Message.ToString()));
                        throw;
                    }
                    catch (Exception ex)
                    {
                        Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "SoftDeleteAddtoCartProduct : errormessage:" + ex.Message.ToString()));
                        throw;
                    }
                }
            }
        }

        // Author: [swathi]
        // Created Date: [05/07/2024]
        // Description: Method for  Insert WishlistProduct
        public async Task<string> InsertWishlistproduct(Wishlistviewmodel wishlistviewmodel)
        {
            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("InsertWishlistItem", sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_Userid", wishlistviewmodel.Userid);
                    cmd.Parameters.AddWithValue("@p_Imageid", wishlistviewmodel.Imageid);
                    cmd.Parameters.AddWithValue("@p_ProductId", wishlistviewmodel.ProductId);

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
                        if (result != null && int.TryParse(result.ToString(), out int newWishlistId))
                        {
                            return "Success";
                        }
                        else
                        {
                            throw new Exception("Failed to add product to wishlist.");
                        }
                    }
                    catch (MySqlException ex)
                    {
                        Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertWishlistProduct : errormessage:" + ex.Message.ToString()));
                        throw;
                    }
                    catch (Exception ex)
                    {
                        Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertWishlistProduct : errormessage:" + ex.Message.ToString()));
                        throw;
                    }
                }
            }
        }
        private async Task<bool> WishlistIsProductAlreadyAdded(MySqlConnection sqlcon, int userId, int imageId, int productId)
        {
            using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM `wishlist` WHERE Userid = @p_Userid AND Imageid = @p_Imageid AND ProductId = @p_ProductId", sqlcon))
            {
                cmd.Parameters.AddWithValue("@p_Userid", userId);
                cmd.Parameters.AddWithValue("@p_Imageid", imageId);
                cmd.Parameters.AddWithValue("@p_ProductId", productId);

                var count = await cmd.ExecuteScalarAsync();
                return Convert.ToInt32(count) > 0;
            }
        }

        // Author: [swathi]
        // Created Date: [05/07/2024]
        // Description: Method for  GetwishlistProduct by userid
        public async Task<IEnumerable<UserProductViewModel>> GetwhislistByUserId(int userId)
        {
            var products = new List<UserProductViewModel>();

            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("GetWishlistByUserId", sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_UserId", userId);

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
                        // Ensure LogFileException and _exPathToSave are defined in your project
                        Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "GetWishlistByUserIdAsync : errormessage:" + ex.Message));
                        throw;
                    }
                }
            }

            return products;
        }
        // Author: [swathi]
        // Created Date: [05/07/2024]
        // Description: Method for  Delete WishListProduct
        public async Task<string> DeleteWishlistproduct(int wishlistid)
        {
            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("SoftDeleteWishlistItem", sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_WishlistId", wishlistid);

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
                    catch (MySqlException ex)
                    {
                        Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "DeleteWishlistProduct : errormessage:" + ex.Message.ToString()));
                        throw;
                    }
                    catch (Exception ex)
                    {
                        Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "DeleteWishlistProduct : errormessage:" + ex.Message.ToString()));
                        throw;
                    }
                }
            }
        }



    }
}
