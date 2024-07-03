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
                return StatusCode(500, $"Internal server error: {ex.Message}"); // Handle exceptions
            }
        }
        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartViewModel addToCartModel)
        {
            try
            {
                var userId = addToCartModel.Userid; // Get from your authentication
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
        // Implement other API endpoints as needed
    }

}

