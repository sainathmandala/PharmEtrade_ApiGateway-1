using BAL.ViewModel;
using System.Threading.Tasks;
using BAL.ViewModels;
using Microsoft.AspNetCore.Http;

namespace BAL.BusinessLogic.Interface
{
    public interface IProductHelper
    {
        Task<string> InsertAddProduct(ProductFilter productviewmodel);
        Task<Productviewmodel> DummyInterface(Productviewmodel pvm);
        Task<string> InsertAddToCartProduct(AddToCartViewModel addToCartModel);
       Task<IEnumerable<UserProductViewModel>> GetByUserId(int userId);
        Task<string> EditProductDetails(int AddproductID,ProductFilter productfilter);

        Task<string> SoftDeleteAddtoCartProduct(int addToCartId);

        Task<string> InsertWishlistproduct(Wishlistviewmodel wishlistviewmodel);
        Task<IEnumerable<UserProductViewModel>> GetwhislistByUserId(int userid);
        Task<string> DeleteWishlistproduct(int wishlistid);

        Task<string> ProcessExcelFileAsync(IFormFile file);



    }
}