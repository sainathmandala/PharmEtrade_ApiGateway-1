using BAL.ViewModel;
using System.Threading.Tasks;
using BAL.ViewModels;
using Microsoft.AspNetCore.Http;
using BAL.Models;
using BAL.ResponseModels;

namespace BAL.BusinessLogic.Interface
{
    public interface IProductHelper
    {
        Task<BAL.ResponseModels.Response<Product>> AddProduct(Product product);
        Task<UploadResponse> UploadImage(IFormFile image);
        Task<BAL.ResponseModels.Response<Product>> GetAllProducts(string productId = null);
        Task<BAL.ResponseModels.Response<Product>> GetProductsBySpecification(int categorySpecificationId);
        Task<BAL.ResponseModels.Response<Product>> GetRecentSoldProducts(int numberOfProducts);
        Task<BAL.ResponseModels.Response<ProductSize>> AddUpdateProductSize(ProductSize productSize);
        Task<string> InsertProductsFromExcel(Stream excelFileStream);
    }
}