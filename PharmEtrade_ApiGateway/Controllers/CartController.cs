using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmEtrade_ApiGateway.Repository.Interface;

namespace PharmEtrade_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private ICartRepository _cartRepository;
        public CartController(ICartRepository cartRepository) { 
            _cartRepository = cartRepository;
        }

        [HttpGet]
        [Route("GetCartItems")]
        public async Task<IActionResult> GetCartItems(string? customerId = null, string? productId = null)
        {
            var response = await _cartRepository.GetCartItems(customerId, productId);
            return Ok(response);
        }
    }
}
