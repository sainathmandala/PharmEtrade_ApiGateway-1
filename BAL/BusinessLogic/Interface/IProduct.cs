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
        Task<BAL.ResponseModels.Response<ProductResponse>> AddProduct(Product product);
        Task<UploadResponse> UploadImage(IFormFile image, string sellerId);
        Task<BAL.ResponseModels.Response<ProductResponse>> GetAllProducts(string productId = null);
        Task<BAL.ResponseModels.Response<ProductResponse>> GetProductsBySpecification(int categorySpecificationId);
        Task<BAL.ResponseModels.Response<ProductResponse>> GetRecentSoldProducts(int numberOfProducts);
        Task<Response<ProductResponse>> GetProductsBySeller(string sellerId);
        Task<BAL.ResponseModels.Response<ProductSize>> AddUpdateProductSize(ProductSize productSize);
        Task<string> InsertProductsFromExcel(Stream excelFileStream);
    }
}