using BAL.BusinessLogic.Interface;
using BAL.ViewModel;
using BAL.ViewModels;
using PharmEtrade_ApiGateway.Repository.Interface;
using System.Threading.Tasks;
using BAL.Common;
using System.IO;

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
        public async Task<Response> InsertAddProduct(ProductFilter productviewmodel, Stream imageFileStream, string imageFileName)
        {
            Response response = new Response();
            try
            {
                string status = await _productHelper.InsertAddProduct(productviewmodel, imageFileStream, imageFileName);
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
        // Created Date: [10/07/2024]
        // Description: Method for BulkInsertProducts
        public async Task<Response> ProcessExcelFileAsync(IFormFile file)
        {
            Response response = new Response();

            if (file == null || file.Length == 0)
            {
                response.status = 400; // Bad Request
                response.message = "No file uploaded.";
                return response;
            }

            try
            {
                // Convert IFormFile to a Stream
                using (var excelFileStream = file.OpenReadStream())
                {
                    string status = await _productHelper.InsertProductsFromExcel(excelFileStream);

                    if (status.Equals("Success"))
                    {
                        response.status = 200; // OK
                        response.message = Constant.InsertAddProductSuccessMsg;
                    }
                    else
                    {
                        response.status = 500; // Internal Server Error
                        response.message = "An error occurred while processing the file.";
                    }
                }
            }
            catch (Exception ex)
            {
                response.status = 500; // Internal Server Error
                response.message = ex.Message;
            }

            return response;
        }



        //Author:[Mamatha]
        //Created Date:[04/07/2024]
        //Description:Method for EditProductDetails
        public async Task<ProductViewModel> EditProductDetails(int AddproductID, ProductFilter productfilter, Stream imageFileStream, string imageFileName)
        {
            ProductViewModel response = new ProductViewModel();
            try
            {
                // Check if ImageUrl is not null and create a memory stream
                Stream memoryStream = Stream.Null;
                if (productfilter.ImageUrl != null)
                {
                    memoryStream = new MemoryStream();
                    await productfilter.ImageUrl.CopyToAsync(memoryStream);
                    memoryStream.Position = 0; // Reset the stream position
                }

                // Call the helper method with the appropriate parameters
                string status = await _productHelper.EditProductDetails(AddproductID, productfilter, memoryStream, productfilter.ImageUrl?.FileName);

                if (status.Equals("Success"))
                {
                    response.statusCode = 200;
                    response.message = Constant.EditProductDetailsMsg;

                }

            }
            catch (Exception ex)
            {
                response.statusCode = 500;
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
        public async Task<List<UserProductViewModel>> GetCartByCustomerID(string CustomerID)
        {
            return await _productHelper.GetCartByCustomerID(CustomerID);

        }
        
        public async Task<Response> SoftDeleteAddtoCartProduct(int addToCartId)
        {
            Response response = new Response();
            try
            {
                string status = await _productHelper.SoftDeleteAddtoCartProduct(addToCartId);
                if (status.Equals("Success"))
                {
                    response.status = 200;
                    response.message = Constant.SoftDeleteAddtoCartProductSuccessMsg;
                }
                else
                {
                    response.status = 500;
                    response.message = "This product is already deleted from cart.";
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
        // Created Date: [05/07/2024]
        // Description: Method for  Insert WishlistProduct
        public async Task<Response> InsertWishlistproduct(Wishlistviewmodel wishlistviewmodel)
        {
            Response response = new Response();
            try
            {
                string status = await _productHelper.InsertWishlistproduct(wishlistviewmodel);
                    
                if (status.Equals("Success"))
                {
                    response.status = 200;
                    response.message = Constant.InsertWishlistproductSuccessMsg;
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
        // Created Date: [05/07/2024]
        // Description: Method for  GetwishlistProduct by userid
        public async Task<IEnumerable<UserProductViewModel>> GetwhislistByUserId(int userId)
        {
            return await _productHelper.GetwhislistByUserId(userId);
        }
        // Author: [swathi]
        // Created Date: [05/07/2024]
        // Description: Method for  Delete WishListProduct
        public async Task<Response> DeleteWishlistproduct(int wishlistid)
        {
            Response response = new Response();
            try
            {
                string status = await _productHelper.DeleteWishlistproduct(wishlistid);
                if (status.Equals("Success"))
                {
                    response.status = 200;
                    response.message = Constant.DeleteWishlistproductSuccessMsg;
                }
                else
                {
                    response.status = 500;
                    response.message = "This product is already deleted from wishlist.";
                }
            }
            catch (Exception ex)
            {
                response.status = 500;
                response.message = ex.Message;
            }
            return response;

        }


    }
}
