using BAL.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmEtrade_ApiGateway.Repository.Interface;

namespace PharmEtrade_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly Iproductsrepo _productRepo;

        public ProductController(Iproductsrepo productRepo)
        {
            _productRepo = productRepo;
        }

        [HttpPost("InsertProduct")]
        public async Task<IActionResult> InsertProduct([FromBody] Productviewmodel productviewmodel)
        {
            try
            {
                var result = await _productRepo.InsertProduct(productviewmodel);
                return Ok(result); // Return appropriate response
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); // Handle exceptions
            }
        }

        // Implement other API endpoints as needed
    }

}

