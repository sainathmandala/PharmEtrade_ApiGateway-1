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


    }
}
