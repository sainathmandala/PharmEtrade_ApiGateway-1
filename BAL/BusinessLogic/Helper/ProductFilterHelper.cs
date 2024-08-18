using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL.BusinessLogic.Interface;
using BAL.Common;
using DAL;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Net.Mime.MediaTypeNames;
using System.Security.AccessControl;
using DAL.Models;
using MySql.Data.MySqlClient;
using BAL.ViewModels;
using BAL.RequestModels;
using System.Data.Common;
namespace BAL.BusinessLogic.Helper
{
    public  class ProductFilterHelper:IProductFilter
    {
        private readonly IsqlDataHelper _isqlDataHelper;
        private readonly string _connectionString;
        private string exFolder = Path.Combine("GetFilteredProductsExceptionLogs");
        private string exPathToSave = string.Empty;

        public ProductFilterHelper(IConfiguration configuration, IsqlDataHelper isqlDataHelper)
        {
            _isqlDataHelper = isqlDataHelper;
            _connectionString = configuration.GetConnectionString("APIDBConnectionString");
            exPathToSave = Path.Combine(Directory.GetCurrentDirectory(), exFolder);
        }
        // Author: [Mamatha]
        // Created Date: [01/07/2024]
        // Description: Method for GetProductFilter
        public async Task<DataTable> GetFilteredProducts(string productName)
        {
            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            using (MySqlCommand cmd = new MySqlCommand("SP_GetFilteredProducts", sqlcon))
            {
                try
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("ProductCategoryID", productCategoryId);
                    cmd.Parameters.AddWithValue("inputProductName", productName);
                    await sqlcon.OpenAsync();
                    DataTable dt = await _isqlDataHelper.SqlDataAdapterasync(cmd);
                    return dt;
                }

                catch (Exception ex)
                {
                    await Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(exPathToSave, "GetFilteredProducts_sp :  errormessage:" + ex.Message.ToString()));
                    throw;
                }
            }
        }

        

        public async Task<List<Products>> GetProducts()
        {
            List<Products> products = new List<Products>();

            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("SP_GetAllProducts", sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        await sqlcon.OpenAsync();

                        using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                Products product = new Products
                                {
                                    AddproductID = reader.GetInt32("AddproductID"),
                                    Productcategory_id = reader.GetInt32("Productcategory_id"),
                                    Sizeid = reader.GetInt32("Sizeid"),
                                    ProductName = reader.GetString("ProductName"),
                                    NDCorUPC = reader.GetString("NDCorUPC"),
                                    BrandName = reader.GetString("BrandName"),
                                    PriceName = reader.GetDecimal("PriceName"),
                                    UPNmemberPrice = reader.GetDecimal("UPNmemberPrice"),
                                    AmountInStock = reader.GetInt32("AmountInStock"),
                                    Taxable = reader.GetBoolean("Taxable"),
                                    SalePrice = reader.GetDecimal("SalePrice"),
                                    SalePriceFrom = reader.GetDateTime("SalePriceFrom"),
                                    SalePriceTo = reader.GetDateTime("SalePriceTo"),
                                    Manufacturer = reader.GetString("Manufacturer"),
                                    Strength = reader.GetString("Strength"),
                                    Fromdate = reader.GetDateTime("Fromdate"),
                                    LotNumber = reader.GetString("LotNumber"),
                                    ExpirationDate = reader.GetDateTime("ExpirationDate"),
                                    PackQuantity = reader.GetInt32("PackQuantity"),
                                    PackType = reader.GetString("PackType"),
                                    PackCondition = reader.GetString("PackCondition"),
                                    ProductDescription = reader.GetString("ProductDescription"),
                                    ImageUrl = reader.GetString("image_url"), // Assuming image_url is the column name for ImageUrl
                                    Caption = reader.GetString("caption"), // Assuming caption is the column name
                                    MetaKeywords = reader.GetString("MetaKeywords"),
                                    MetaTitle = reader.GetString("MetaTitle"),
                                    MetaDescription = reader.GetString("MetaDescription"),
                                    SaltComposition = reader.GetString("SaltComposition"),
                                    UriKey = reader.GetString("UriKey"),
                                    AboutTheProduct = reader.GetString("AboutTheProduct"),
                                    CategorySpecificationId = reader.GetInt32("CategorySpecificationId"),
                                    ProductTypeId = reader.GetInt32("ProductTypeId"),
                                    SellerId = reader.GetString("SellerId")
                                };

                                products.Add(product);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        await Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(exPathToSave, "GetAllProducts_sp : errormessage:" + ex.Message));
                        throw;
                    }
                }
            }

            return products;
        }

        public async Task<DataTable> GetProductsById(int AddproductID)
        {
            MySqlConnection sqlcon = new MySqlConnection(_connectionString);
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                cmd = new MySqlCommand("SP_GetProductById", sqlcon);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("in_AddproductID", AddproductID);

                return await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmd));
            }
            catch(Exception ex)
            {
                Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(exPathToSave, "GetProductsById_sp:errormessage:" + ex.Message.ToString()));
                throw ex;
            }
        }






    }  
}
