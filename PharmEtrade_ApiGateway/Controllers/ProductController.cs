using BAL.ViewModel;
using BAL.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmEtrade_ApiGateway.Repository.Interface;

namespace PharmEtrade_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductsRepo _productRepo;

        public ProductController(IProductsRepo productRepo)
        {
            _productRepo = productRepo;
        }

        [HttpPost("InsertProduct")]
        public async Task<IActionResult> InsertProduct([FromBody] ProductFilter productviewmodel)
        {
            try
            {
                var result = await _productRepo.InsertAddProduct(productviewmodel);
                return Ok(result); // Return appropriate response
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); 
            }
        }
        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartViewModel addToCartModel)
        {
            try
            {
                var userId = addToCartModel.Userid; 
                var imageId = addToCartModel.Imageid;
                var productId = addToCartModel.ProductId;

                var result = await _productRepo.InsertAddToCartProduct(addToCartModel);

                return Ok(new { AddToCartId = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("GetByUserId/{userId}")]
        public async Task<ActionResult<IEnumerable<UserProductViewModel>>> GetByUserId(int userId)
        {
            var products = await _productRepo.GetByUserId(userId);
            return Ok(products);
        }

        [HttpPost("SoftDeleteAddtoCartProduct")]
        public async Task<IActionResult> SoftDeleteAddtoCartProduct([FromBody] int addToCartId)
        {
            try
            {
                var result = await _productRepo.SoftDeleteAddtoCartProduct(addToCartId);
                if (result.status == 200)
                {
                    return Ok(result);
                }
                else
                {
                    return StatusCode(500, result.message);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("AddWishlist")]
        public async Task<IActionResult> AddWishlist([FromBody] Wishlistviewmodel wishlistviewmodel)
        {
            try
            {
                var userId = wishlistviewmodel.Userid;
                var imageId = wishlistviewmodel.Imageid;
                var productId = wishlistviewmodel.ProductId;

                var result = await _productRepo.InsertWishlistproduct(wishlistviewmodel);

                return Ok(new { Wishlistid = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("GetwishlistByUserId/{userId}")]
        public async Task<ActionResult<IEnumerable<UserProductViewModel>>> GetwishlistByUserId(int userId)
        {
            var products = await _productRepo.GetwhislistByUserId(userId);
            return Ok(products);
        }


        [HttpPost("DeleteWishlistProduct")]
        public async Task<IActionResult> DeleteWishlistProduct([FromBody] int wishlistid)
        {
            try
            {
                var result = await _productRepo.DeleteWishlistproduct(wishlistid);
                if (result.status == 200)
                {
                    return Ok(result);
                }
                else
                {
                    return StatusCode(500, result.message);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }

}

