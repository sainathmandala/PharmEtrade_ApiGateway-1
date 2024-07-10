using BAL.ViewModel;
using BAL.ViewModels;
using System.Threading.Tasks;

namespace PharmEtrade_ApiGateway.Repository.Interface
{
    public interface IProductsRepo
    {
        Task<Response> InsertAddProduct(ProductFilter productviewmodel);
        Task<Response> InsertAddToCartProduct(AddToCartViewModel addToCartModel);
        Task<IEnumerable<UserProductViewModel>> GetByUserId(int userId);
        Task<ProductViewModel> EditProductDetails(int AddproductID, ProductFilter productfilter);

        Task<Response> SoftDeleteAddtoCartProduct(int addToCartId);
        Task<Response> InsertWishlistproduct(Wishlistviewmodel wishlistviewmodel);
        Task<IEnumerable<UserProductViewModel>> GetwhislistByUserId(int userId);
        Task<Response> DeleteWishlistproduct(int wishlistid);
        Task<Response> ProcessExcelFileAsync(IFormFile file);

    }
}
