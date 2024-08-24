using BAL.Models;
using BAL.ResponseModels;
using BAL.ViewModel;
using BAL.ViewModels;
using System.Threading.Tasks;

namespace PharmEtrade_ApiGateway.Repository.Interface
{
    public interface IProductsRepo
    {
        Task<BAL.ResponseModels.Response<Product>> AddProduct(Product product);
        Task<UploadResponse> UploadImage(IFormFile image);
        Task<BAL.ResponseModels.Response<Product>> GetAllProducts(string productId = null);
        Task<BAL.ResponseModels.Response<Product>> GetProductsBySpecification(int categorySpecificationId);
        Task<BAL.ResponseModels.Response<Product>> GetRecentSoldProducts(int numberOfProducts);
        Task<BAL.ResponseModels.Response<ProductSize>> AddUpdateProductSize(ProductSize productSize);        
        Task<Response> ProcessExcelFileAsync(IFormFile file);

    }
}
