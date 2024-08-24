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
        Task<string> InsertAddProduct(ProductFilter productviewmodel, Stream imageFileStream, string imageFileName);
        Task<string> InsertProductsFromExcel(Stream excelFileStream);
        Task<Productviewmodel> DummyInterface(Productviewmodel pvm);
        Task<string> InsertAddToCartProduct(AddToCartViewModel addToCartModel);
      // Task<IEnumerable<UserProductViewModel>> GetCartByUserId(int userId);
        Task<string> EditProductDetails(int AddproductID,ProductFilter productfilter, Stream imageFileStream, string imageFileName);

        Task<string> SoftDeleteAddtoCartProduct(int addToCartId);

        Task<string> InsertWishlistproduct(Wishlistviewmodel wishlistviewmodel);
        Task<IEnumerable<UserProductViewModel>> GetwhislistByUserId(int userid);
        Task<string> DeleteWishlistproduct(int wishlistid);

        //Task<string> ProcessExcelFileAsync(IFormFile file);

        Task<List<UserProductViewModel>> GetCartByCustomerID(string CustomerID);



    }
}