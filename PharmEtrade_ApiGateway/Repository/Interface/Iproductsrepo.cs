using BAL.Models;
using BAL.Models.Products;
using BAL.RequestModels;
using BAL.ResponseModels;
using BAL.ViewModel;
using BAL.ViewModels;
using System.Threading.Tasks;

namespace PharmEtrade_ApiGateway.Repository.Interface
{
    public interface IProductsRepo
    {
        Task<BAL.ResponseModels.Response<ProductResponse>> AddProduct(Product product);
        Task<BAL.ResponseModels.Response<ProductInfo>> AddUpdateProductInfo(ProductInfo productInfo);
        Task<BAL.ResponseModels.Response<ProductPrice>> AddUpdateProductPrice(ProductPrice productPrice);
        Task<BAL.ResponseModels.Response<ProductGallery>> AddUpdateProductGallery(ProductGallery productGallery);
        Task<UploadResponse> UploadImage(IFormFile image, string sellerId, string productId);
   
             Task<BAL.ResponseModels.Response<ProductResponse>> AddBulkProduct(IFormFile excelfile);
        Task<BAL.ResponseModels.Response<ProductResponse>> GetAllProducts(string productId = null, string customerId = null);
        Task<BAL.ResponseModels.Response<ProductResponse>> GetProductsBySpecification(int categorySpecificationId, bool withDiscount = false);
        Task<BAL.ResponseModels.Response<ProductResponse>> GetRecentSoldProducts(int numberOfProducts);
        Task<Response<ProductResponse>> GetProductsBySeller(string sellerId);
        Task<Response<ProductResponse>> GetProductsByCriteria(ProductCriteria criteria);
        Task<BAL.ResponseModels.Response<ProductSize>> AddUpdateProductSize(ProductSize productSize);
        Task<Response> ProcessExcelFileAsync(IFormFile file);
        Task<BAL.ResponseModels.Response<SpecialOffersResponse>> GetSpecialOffers();

        Task<BAL.ResponseModels.Response<SpecialOffersResponse>> GetSpecialOffersbyproductCategory();

        Task<Response<ProductResponse>> GetRelatedProducts(string productId);
        Task<Response<ProductResponse>> GetUpsellProducts(string productId);
        Task<Response<ProductResponse>> GetCrossSellProducts(string productId);
        Task<Response<string>> AddRelatedProduct(string productId, string relatedProductId);
        Task<Response<string>> AddUpsellProduct(string productId, string upsellProductId);
        Task<Response<string>> AddCrossSellProduct(string productId, string crossSellProductId);
        Task<Response<string>> RemoveRelatedProduct(string productId, string relatedProductId);
        Task<Response<string>> RemoveUpsellProduct(string productId, string upsellProductId);
        Task<Response<string>> RemoveCrossSellProduct(string productId, string crossSellProductId);
        Task<BAL.ResponseModels.Response<ProductRating>> AddProductRating(ProductRating productRating);
        Task<BAL.ResponseModels.Response<ProductRating>> UpdateProductRating(ProductRating productrating);
        Task<Response<ProductRating>> GetRatingwithProduct(string ProductID);
        Task<Response<string>> RemoveProductRating(string RatingID);
        Task<Response<ProductRating>> GetRating(string RatingID);
        Task<Response<ProductsPerCategory>> GetProductsPerCategoryCounts(string sellerId = "");
        Task<Response<string>> DeactivateProduct(string productId);
        Task<Response<string>> DeleteProduct(string productId);


    }
}
