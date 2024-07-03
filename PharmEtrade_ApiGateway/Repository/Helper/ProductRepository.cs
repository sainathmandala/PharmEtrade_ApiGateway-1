using BAL.BusinessLogic.Interface;
using BAL.ViewModel;
using BAL.ViewModels;
using PharmEtrade_ApiGateway.Repository.Interface;
using System.Threading.Tasks;
using BAL.Common;

namespace PharmEtrade_ApiGateway.Repository.Helper
{
    public class ProductRepository : IProductsRepo
    {
        private readonly IProductHelper _productHelper;

        public ProductRepository(IProductHelper productHelper)
        {
            _productHelper = productHelper;
        }

        // Author: [swathi]
        // Created Date: [02/07/2024]
        // Description: Method for InsertProducts
        public async Task<Response> InsertAddProduct(ProductFilter productviewmodel)
        {
            Response response = new Response();
            try
            {

                // Add any business logic or validation here if necessary
                string status = await _productHelper.InsertAddProduct(productviewmodel);
                if (status.Equals("Success"))
                {
                    response.status = 200;
                    response.message = Constant.InsertAddProductSuccessMsg;
                }
            }
            catch (Exception ex)
            {
                response.status = 500;
                response.message = ex.Message;

            }
            return response;

        }

        // Author: [swathi]
        // Created Date: [02/07/2024]
        // Description: Method for AddtoCartProducts
        public async Task<Response> InsertAddToCartProduct(AddToCartViewModel addToCartModel)
        {
            Response response = new Response();
            try
            {
                string status = await _productHelper.InsertAddToCartProduct(addToCartModel);
                if (status.Equals("Success"))
                {
                    response.status = 200;
                    response.message = Constant.InsertAddToCartProductSuccessMsg;
                }
            }
            catch (Exception ex)
            {
                response.status = 500;
                response.message = ex.Message;

            }
            return response;
        }
        // Author: [swathi]
        // Created Date: [03/07/2024]
        // Description: Method for GetCartProducts based on userid
        public async Task<IEnumerable<UserProductViewModel>> GetByUserId(int userId)
        {
            return await _productHelper.GetByUserId(userId);
        }


    }
}
