using BAL.Models;
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
        Task<BAL.ResponseModels.Response<ProductResponse>> GetAllProducts(string productId = null);
        Task<BAL.ResponseModels.Response<ProductResponse>> GetProductsBySpecification(int categorySpecificationId);
        Task<BAL.ResponseModels.Response<ProductResponse>> GetRecentSoldProducts(int numberOfProducts);
        Task<Response<ProductResponse>> GetProductsBySeller(string sellerId);
        Task<Response<ProductResponse>> GetProductsByCriteria(ProductCriteria criteria);
        Task<BAL.ResponseModels.Response<ProductSize>> AddUpdateProductSize(ProductSize productSize);
        Task<Response> ProcessExcelFileAsync(IFormFile file);
        Task<BAL.ResponseModels.Response<SpecialOffersResponse>> GetSpecialOffers();


    }
}
