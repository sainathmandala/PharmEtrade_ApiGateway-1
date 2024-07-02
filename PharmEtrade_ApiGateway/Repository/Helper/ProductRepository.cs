using BAL.BusinessLogic.Interface;
using BAL.ViewModel;
using PharmEtrade_ApiGateway.Repository.Interface;
using System.Threading.Tasks;

namespace PharmEtrade_ApiGateway.Repository.Helper
{
    public class ProductRepository : IProductsRepo
    {
        private readonly IProductHelper _productHelper;

        public ProductRepository(IProductHelper productHelper)
        {
            _productHelper = productHelper;
        }

        public async Task<int> InsertAddProduct(Productviewmodel productviewmodel)
        {
            // Add any business logic or validation here if necessary
            return await _productHelper.InsertAddProduct(productviewmodel);
        }

       public async Task<int> InsertAddToCartProduct(AddToCartViewModel addToCartModel)
        {
            return await _productHelper.InsertAddToCartProduct(addToCartModel );
        }

       
    }
}
