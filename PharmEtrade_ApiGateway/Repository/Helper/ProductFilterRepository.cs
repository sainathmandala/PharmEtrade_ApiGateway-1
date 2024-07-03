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
        public async Task<List<ProductFilter>> GetFilteredProducts(int? productCategoryId, string productName)
        {
            List<ProductFilter> response = new List<ProductFilter>();

            try
            {
                var dtResult = await _IProductFilter.GetFilteredProducts(productCategoryId, productName);

                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    foreach (DataRow row in dtResult.Rows)
                    {
                        response.Add(new ProductFilter
                        {
                            AddproductID = Convert.ToInt32(row["AddproductID"]),
                            Productcategory_id = Convert.ToInt32(row["Productcategory_id"]),
                            ImageID = Convert.ToInt32(row["ImageID"]),
                            Sizeid = Convert.ToInt32(row["Sizeid"]),
                            ProductName = row["ProductName"].ToString(),
                            NDCorUPC = row["NDCorUPC"].ToString(),
                            BrandName = row["BrandName"].ToString(),
                            PriceName = Convert.ToDecimal(row["PriceName"]),
                            UPNmemberPrice = Convert.ToDecimal(row["UPNmemberPrice"]),
                            AmountInStock = Convert.ToInt32(row["AmountInStock"]),
                            Taxable = Convert.ToBoolean(row["Taxable"]),
                            SalePrice = Convert.ToDecimal(row["SalePrice"]),
                            SalePriceFrom = Convert.ToDateTime(row["SalePriceFrom"]),
                            SalePriceTo = Convert.ToDateTime(row["SalePriceTo"]),
                            Manufacturer = row["Manufacturer"].ToString(),
                            Strength = row["Strength"].ToString(),
                            Fromdate = Convert.ToDateTime(row["Fromdate"]),
                            LotNumber = row["LotNumber"].ToString(),
                            ExpirationDate = Convert.ToDateTime(row["ExpirationDate"]),
                            PackQuantity = Convert.ToInt32(row["PackQuantity"]),
                            PackType = row["PackType"].ToString(),
                            PackCondition = row["PackCondition"].ToString(),
                            ProductDescription = row["ProductDescription"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, and/or return an appropriate error response.
                throw new Exception($"An error occurred while fetching products: {ex.Message}");
            }

            return response;
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
            List<ProductFilter> productfilter = new List<ProductFilter>();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    ProductFilter user = new ProductFilter();
                    user.AddproductID = Convert.ToInt32(row["AddproductID"]);
                    user.Productcategory_id = Convert.ToInt32(row["Productcategory_id"]);
                    user.ImageID = Convert.ToInt32(row["ImageID"]);
                    user.Sizeid = Convert.ToInt32(row["Sizeid"]);
                    user.ProductName = row["ProductName"].ToString();
                    user.NDCorUPC = row["NDCorUPC"].ToString();
                    user.BrandName = row["BrandName"].ToString();
                    user.PriceName = Convert.ToDecimal(row["PriceName"]);
                    user.UPNmemberPrice = Convert.ToDecimal(row["UPNmemberPrice"]);
                    user.AmountInStock = Convert.ToInt32(row["AmountInStock"]);
                    user.Taxable = Convert.ToBoolean(row["Taxable"]);
                    user.SalePrice = Convert.ToDecimal(row["SalePrice"]);
                    user.SalePriceFrom = Convert.ToDateTime(row["SalePriceFrom"]);
                    user.SalePriceTo = Convert.ToDateTime(row["SalePriceTo"]);
                    user.Manufacturer = row["Manufacturer"].ToString();
                    user.Strength = row["Strength"].ToString();
                    user.Fromdate = Convert.ToDateTime(row["Fromdate"]);
                    user.LotNumber = row["LotNumber"].ToString();
                    user.ExpirationDate = Convert.ToDateTime(row["ExpirationDate"]);
                    user.PackQuantity = Convert.ToInt32(row["PackQuantity"]);
                    user.PackType = row["PackType"].ToString();
                    user.PackCondition = row["PackCondition"].ToString();
                    user.ProductDescription = row["ProductDescription"].ToString();


                    productfilter.Add(user);

                }
                return productfilter;
            }
            else

                return productfilter;
        }
    }

}