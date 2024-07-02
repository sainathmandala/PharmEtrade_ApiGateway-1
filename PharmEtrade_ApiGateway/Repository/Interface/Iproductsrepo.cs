using BAL.ViewModel;
using System.Threading.Tasks;

namespace PharmEtrade_ApiGateway.Repository.Interface
{
    public interface IProductsRepo
    {
        Task<int> InsertAddProduct(Productviewmodel productviewmodel);
        Task<int> InsertAddToCartProduct(AddToCartViewModel addToCartModel);


    }
}
