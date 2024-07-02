using BAL.ViewModel;
using System.Threading.Tasks;

namespace BAL.BusinessLogic.Interface
{
    public interface IProductHelper
    {
        Task<int> InsertAddProduct(Productviewmodel productviewmodel);
        Task<Productviewmodel> DummyInterface(Productviewmodel pvm);
        Task<int> InsertAddToCartProduct(AddToCartViewModel addToCartModel);



    }
}