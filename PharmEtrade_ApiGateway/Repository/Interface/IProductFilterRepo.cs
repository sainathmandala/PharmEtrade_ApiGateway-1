using BAL.ViewModels;

namespace PharmEtrade_ApiGateway.Repository.Interface
{
    public interface IProductFilterRepo
    {
        Task<List<ProductFilter>> GetFilteredProducts(int? productCategoryId, string productName);
        Task<ProductViewModel> GetProducts();

    }
}
