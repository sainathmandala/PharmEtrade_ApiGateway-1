using BAL.BusinessLogic.Interface;
using BAL.Common;
using BAL.ViewModel;
using DAL;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace BAL.BusinessLogic.Helper
{
    public class ProductHelper:IProduct
    {
        private readonly IsqlDataHelper _isqlDataHelper;
        private readonly string _connectionString;
        private string exFolder = Path.Combine("CustomerExceptionLogs");
        private string exPathToSave = string.Empty;

        public ProductHelper(IConfiguration configuration, IsqlDataHelper isqlDataHelper)
        {
            _isqlDataHelper = isqlDataHelper;
            _connectionString = configuration.GetConnectionString("OnlineexamDB");
            exPathToSave = Path.Combine(Directory.GetCurrentDirectory(), exFolder);
        }




        public async Task<Productviewmodel> InsertAddProduct(Productviewmodel productviewmodel)
        {
            using (SqlConnection sqlcon = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertAddProduct", sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                   
                    cmd.Parameters.AddWithValue("@Productcategory_id", productviewmodel.ProductcategoryId);
                    cmd.Parameters.AddWithValue("@ImageID", productviewmodel.ImageId);
                    cmd.Parameters.AddWithValue("@Sizeid", productviewmodel.Sizeid);
                    cmd.Parameters.AddWithValue("@ProductName", productviewmodel.ProductName);
                    cmd.Parameters.AddWithValue("@NDCorUPC", productviewmodel.NdcorUpc);
                    cmd.Parameters.AddWithValue("@BrandName", productviewmodel.BrandName);
                    cmd.Parameters.AddWithValue("@PriceName", productviewmodel.PriceName);
                    cmd.Parameters.AddWithValue("@UPNmemberPrice", productviewmodel.UpnmemberPrice);
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


                    // Set parameters...

                    try
                    {
                        await sqlcon.OpenAsync();
                        var result = await cmd.ExecuteNonQueryAsync();
                        return productviewmodel; // Return appropriate result or handle accordingly
                    }
                    catch (Exception ex)
                    {
                        throw; // Re-throw exception to maintain stack trace
                    }
                }
            }
        }

       
    }
}
