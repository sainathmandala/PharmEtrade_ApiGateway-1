using BAL.ViewModel;

namespace PharmEtrade_ApiGateway.Repository.Interface
{
    public interface Iproductsrepo
    {

        Task<int> InsertProduct(Productviewmodel productviewmodel);
    }
}
