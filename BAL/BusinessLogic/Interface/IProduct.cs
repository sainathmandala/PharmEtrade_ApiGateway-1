using BAL.ViewModel;
using System.Threading.Tasks;
using BAL.ViewModels;

namespace BAL.BusinessLogic.Interface
{
    public interface IProductHelper
    {
        Task<string> InsertAddProduct(ProductFilter productviewmodel);
        Task<Productviewmodel> DummyInterface(Productviewmodel pvm);
        Task<string> InsertAddToCartProduct(AddToCartViewModel addToCartModel);
       Task<IEnumerable<UserProductViewModel>> GetByUserId(int userId);

    }
}