using BAL.ViewModel;
using DAL;
using PharmEtrade_ApiGateway.Repository.Interface;
using System.Data;
using System.Data.SqlClient;

namespace PharmEtrade_ApiGateway.Repository.Helper
{
    public class ProductRepository:Iproductsrepo
    {
        private readonly IsqlDataHelper _sqlDataHelper;
        private readonly string _connectionString;

        public ProductRepository(IsqlDataHelper sqlDataHelper, IConfiguration configuration)
        {
            _sqlDataHelper = sqlDataHelper;
            _connectionString = configuration.GetConnectionString("OnlineexamDB");
        }

        public async Task<int> InsertProduct(Productviewmodel productviewmodel)
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

                    try
                    {
                        await sqlcon.OpenAsync();
                        var result = await cmd.ExecuteNonQueryAsync();
                        return result; 
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
