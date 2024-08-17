using BAL.RequestModels;
using BAL.ViewModels;

namespace PharmEtrade_ApiGateway.Repository.Interface
{
    public interface IProductFilterRepo
    {
        Task<List<ProductFilter>> GetFilteredProducts(string productName);
        Task<List<Products>> GetProducts();
        Task<ProductViewModel> GetProductsById(int AddproductID);

    }
}
