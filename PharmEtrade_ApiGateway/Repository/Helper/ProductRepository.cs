using BAL.BusinessLogic.Interface;
using BAL.ViewModel;
using BAL.ViewModels;
using PharmEtrade_ApiGateway.Repository.Interface;
using System.Threading.Tasks;
using BAL.Common;
using System.IO;
using BAL.Models;
using BAL.BusinessLogic.Helper;
using BAL.ResponseModels;
using BAL.RequestModels;
using BAL.Models.Products;

namespace PharmEtrade_ApiGateway.Repository.Helper
{
    public class ProductRepository : IProductsRepo
    {
        private readonly IProductHelper _productHelper;

        public ProductRepository(IProductHelper productHelper)
        {
            _productHelper = productHelper;
        }

        public async Task<Response> ProcessExcelFileAsync(IFormFile file)
        {
            Response response = new Response();

            if (file == null || file.Length == 0)
            {
                response.status = 400; // Bad Request
                response.message = "No file uploaded.";
                return response;
            }

            try
            {
                // Convert IFormFile to a Stream
                using (var excelFileStream = file.OpenReadStream())
                {
                    string status = await _productHelper.InsertProductsFromExcel(excelFileStream);

                    if (status.Equals("Success"))
                    {
                        response.status = 200; // OK
                        response.message = Constant.InsertAddProductSuccessMsg;
                    }
                    else
                    {
                        response.status = 500; // Internal Server Error
                        response.message = "An error occurred while processing the file.";
                    }
                }
            }
            catch (Exception ex)
            {
                response.status = 500; // Internal Server Error
                response.message = ex.Message;
            }

            return response;
        }

        public async Task<BAL.ResponseModels.Response<ProductResponse>> AddProduct(Product product)
        {
            return await _productHelper.AddProduct(product);
        }

        public async Task<UploadResponse> UploadImage(IFormFile image, string sellerId, string productId)
        {
            UploadResponse response = new UploadResponse();
            try
            {
                response = await _productHelper.UploadImage(image, sellerId, productId);
            }
            catch (Exception ex)
            {
                //response.Status = 500;
                //response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Response<ProductResponse>> GetAllProducts(string productId = null)
        {
            return await _productHelper.GetAllProducts(productId);
        }

        public async Task<Response<ProductResponse>> GetProductsBySpecification(int categorySpecificationId, bool withDiscount = false)
        {
            return await _productHelper.GetProductsBySpecification(categorySpecificationId, withDiscount);
        }

        public async Task<Response<ProductResponse>> GetRecentSoldProducts(int numberOfProducts)
        {
            return await _productHelper.GetRecentSoldProducts(numberOfProducts);
        }

        public async Task<Response<ProductSize>> AddUpdateProductSize(ProductSize productSize)
        {
            return await _productHelper.AddUpdateProductSize(productSize);
        }

        public async Task<Response<ProductResponse>> GetProductsBySeller(string sellerId)
        {
            return await _productHelper.GetProductsBySeller(sellerId);
        }

        public async Task<Response<ProductResponse>> GetProductsByCriteria(ProductCriteria criteria)
        {
            return await _productHelper.GetProductsByCriteria(criteria);
        }

        public async Task<Response<ProductInfo>> AddUpdateProductInfo(ProductInfo productInfo)
        {
            return await _productHelper.AddUpdateProductInfo(productInfo);
        }

        public async Task<Response<ProductRating>> AddProductRating(ProductRating productRating)
        {
            return await _productHelper.AddProductRating(productRating);
        }
        public async Task<Response<ProductRating>> UpdateProductRating(ProductRating productRating)
        {
            return await _productHelper.UpdateProductRating(productRating);
        }

        public async Task<Response<ProductRating>> GetRatingwithProduct(string productId)
        {
            return await _productHelper.GetRatingwithProduct(productId);
        }
        
        public async Task<Response<string>> RemoveProductRating(string RatingID)
        {
            return await _productHelper.RemoveProductRating(RatingID);
        }


        public async Task<Response<ProductPrice>> AddUpdateProductPrice(ProductPrice productPrice)
        {
            return await _productHelper.AddUpdateProductPrice(productPrice);
        }

        public async Task<Response<ProductGallery>> AddUpdateProductGallery(ProductGallery productGallery)
        {
            return await _productHelper.AddUpdateProductGallery(productGallery);
        }
        public async Task<Response<SpecialOffersResponse>> GetSpecialOffers()
        {
            return await _productHelper.GetSpecialOffers();
        }

        public async Task<Response<ProductResponse>> GetRelatedProducts(string productId)
        {
            return await _productHelper.GetRelatedProducts(productId);
        }

        public async Task<Response<ProductResponse>> GetUpsellProducts(string productId)
        {
            return await _productHelper.GetUpsellProducts(productId);
        }

        public async Task<Response<ProductResponse>> GetCrossSellProducts(string productId)
        {
            return await _productHelper.GetCrossSellProducts(productId);
        }

        public async Task<Response<string>> AddRelatedProduct(string productId, string relatedProductId)
        {
            return await _productHelper.AddRelatedProduct(productId, relatedProductId);
        }
        


        public async Task<Response<string>> AddUpsellProduct(string productId, string upsellProductId)
        {
            return await _productHelper.AddUpsellProduct(productId, upsellProductId);
        }

        public async Task<Response<string>> AddCrossSellProduct(string productId, string crossSellProductId)
        {
            return await _productHelper.AddCrossSellProduct(productId, crossSellProductId);
        }

        public async Task<Response<string>> RemoveRelatedProduct(string productId, string relatedProductId)
        {
            return await _productHelper.RemoveRelatedProduct(productId, relatedProductId);
        }

        public async Task<Response<string>> RemoveUpsellProduct(string productId, string upsellProductId)
        {
            return await _productHelper.RemoveUpsellProduct(productId, upsellProductId);
        }

        public async Task<Response<string>> RemoveCrossSellProduct(string productId, string crossSellProductId)
        {
            return await _productHelper.RemoveCrossSellProduct(productId, crossSellProductId);
        }

        public async Task<Response<ProductRating>> GetRating(string RatingID)
        {
            return await _productHelper.GetRatingbyId(RatingID);
        }

        public async Task<Response<ProductsPerCategory>> GetProductsPerCategoryCounts(string sellerId = "")
        {
            return await _productHelper.GetProductsPerCategoryCounts(sellerId);
        }

        public async Task<Response<string>> DeactivateProduct(string productId)
        {
            return await _productHelper.DeactivateProduct(productId);
        }
        public async Task<Response<string>> DeleteProduct(string productId)
        {
            return await _productHelper.DeleteProduct(productId);
        }


    }
}
