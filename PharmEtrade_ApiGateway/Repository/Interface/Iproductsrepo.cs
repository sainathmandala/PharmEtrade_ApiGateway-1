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
        Task<Response> InsertAddProduct(ProductFilter productviewmodel, Stream imageFileStream, string imageFileName);
        Task<Response> InsertAddToCartProduct(AddToCartViewModel addToCartModel);
        Task<List<UserProductViewModel>> GetCartByCustomerID(string CustomerID);

        Task<ProductViewModel> EditProductDetails(int AddproductID, ProductFilter productfilter,Stream imageFileStream,string imageFileName);

        Task<Response> SoftDeleteAddtoCartProduct(int addToCartId);
        Task<Response> InsertWishlistproduct(Wishlistviewmodel wishlistviewmodel);
        Task<IEnumerable<UserProductViewModel>> GetwhislistByUserId(int userId);
        Task<Response> DeleteWishlistproduct(int wishlistid);
        Task<Response> ProcessExcelFileAsync(IFormFile file);

    }
}
