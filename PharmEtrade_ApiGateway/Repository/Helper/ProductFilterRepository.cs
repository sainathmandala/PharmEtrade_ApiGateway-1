using BAL.BusinessLogic.Interface;
using BAL.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PharmEtrade_ApiGateway.Repository.Interface;
using System.Data;


using BAL.Common;


namespace PharmEtrade_ApiGateway.Repository.Helper
{
    public class ProductFilterRepository : IProductFilterRepo
    {
        private readonly IProductFilter _IProductFilter;

        public ProductFilterRepository(IProductFilter iproductfilter)
        {
            _IProductFilter = iproductfilter;
        }
        public async Task<List<ProductFilter>> GetFilteredProducts(string productName)
        {
            try
            {
                var dtResult = await _IProductFilter.GetFilteredProducts(productName);

                var response = ConvertDataTabletoProductList(dtResult);

                return response;
            }

            catch (Exception ex)
            {
                // Handle the exception, log it, and/or return an appropriate error response.
                throw new Exception($"An error occurred while fetching products: {ex.Message}");
            }


        }


        // Author: [Mamatha]
        // Created Date: [02/07/2024]
        // Description: Method for GetProducts
        public async  Task<ProductViewModel> GetProducts()
        {
            ProductViewModel response = new ProductViewModel();
            try
            {
                DataTable dtresult = await _IProductFilter.GetProducts();
                if (dtresult != null && dtresult.Rows.Count > 0)
                {

                    response.statusCode = 200;
                    response.message = Constant.GetProductSuccessMsg;
                   response.Productfilter = ConvertDataTabletoProductList(dtresult);

                }
            }
            catch (Exception ex)
            {
                response.statusCode = 500;
                response.message = ex.Message;
                response.Productfilter = new List<ProductFilter>();

            }
            return response;
        }
        public async Task<ProductViewModel> GetProductsById(int AddproductID)
        {
            ProductViewModel response = new ProductViewModel();
            try
            {
                DataTable dt = await _IProductFilter.GetProductsById(AddproductID);
                if(dt!=null && dt.Rows.Count > 0)
                {
                    response.statusCode = 200;
                    response.message = Constant.GetProductsByIdSuccessMsg;
                    response.Productfilter = ConvertDataTabletoProductList(dt);


                }
            }
            catch(Exception ex)
            {
                response.statusCode = 500;
                response.message = ex.Message;
                response.Productfilter = new List<ProductFilter>();
            }
            return response;
        }


        private List<ProductFilter> ConvertDataTabletoProductList(DataTable dt)
        {
            List<ProductFilter> productFilterList = new List<ProductFilter>();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    ProductFilter productFilter = new ProductFilter();

                    productFilter.AddproductID = dt.Columns.Contains("AddproductID") && row["AddproductID"] != DBNull.Value ? Convert.ToInt32(row["AddproductID"]) : 0;
                    productFilter.Productcategory_id = row["Productcategory_id"] != DBNull.Value ? Convert.ToInt32(row["Productcategory_id"]) : 0;
                    productFilter.Sizeid = row["Sizeid"] != DBNull.Value ? Convert.ToInt32(row["Sizeid"]) : 0;
                    productFilter.ProductName = row["ProductName"].ToString();
                    productFilter.NDCorUPC = row["NDCorUPC"].ToString();
                    productFilter.BrandName = row["BrandName"].ToString();
                    productFilter.PriceName = row["PriceName"] != DBNull.Value ? Convert.ToDecimal(row["PriceName"]) : 0;
                    productFilter.UPNmemberPrice = row["UPNmemberPrice"] != DBNull.Value ? Convert.ToDecimal(row["UPNmemberPrice"]) : 0;
                    productFilter.AmountInStock = row["AmountInStock"] != DBNull.Value ? Convert.ToInt32(row["AmountInStock"]) : 0;
                    productFilter.Taxable = row["Taxable"] != DBNull.Value ? Convert.ToBoolean(row["Taxable"]) : false;
                    productFilter.SalePrice = row["SalePrice"] != DBNull.Value ? Convert.ToDecimal(row["SalePrice"]) : 0;
                    productFilter.SalePriceFrom = Convert.ToDateTime(row["SalePriceFrom"]);
                    productFilter.SalePriceTo = Convert.ToDateTime(row["SalePriceTo"]);
                    productFilter.Manufacturer = row["Manufacturer"].ToString();
                    productFilter.Strength = row["Strength"].ToString();
                    productFilter.Fromdate = Convert.ToDateTime(row["Fromdate"]);
                    productFilter.LotNumber = row["LotNumber"].ToString();
                    productFilter.ExpirationDate = Convert.ToDateTime(row["ExpirationDate"]);
                    productFilter.PackQuantity = row["PackQuantity"] != DBNull.Value ? Convert.ToInt32(row["PackQuantity"]) : 0;
                    productFilter.PackType = row["PackType"].ToString();
                    productFilter.PackCondition = row["PackCondition"].ToString();
                    productFilter.ProductDescription = row["ProductDescription"].ToString();
                    productFilter.MetaKeywords = row["MetaKeywords"].ToString();
                    productFilter.MetaTitle = row["MetaTitle"].ToString();
                    productFilter.MetaDescription = row["MetaDescription"].ToString();
                    productFilter.SaltComposition = row["SaltComposition"].ToString();
                    productFilter.UriKey = row["UriKey"].ToString();
                    productFilter.AboutTheProduct = row["AboutTheProduct"].ToString();
                    productFilter.CategorySpecificationId = row["CategorySpecificationId"] != DBNull.Value ? Convert.ToInt32(row["CategorySpecificationId"]) : 0;
                    productFilter.ProductTypeId = row["ProductTypeId"] != DBNull.Value ? Convert.ToInt32(row["ProductTypeId"]) : 0;
                    productFilter.SellerId = row["SellerId"] != DBNull.Value ? Convert.ToInt32(row["SellerId"]) : 0;

                    productFilterList.Add(productFilter);
                }

            }


            return productFilterList;
        }


    }

}