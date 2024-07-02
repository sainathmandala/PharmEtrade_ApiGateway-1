using BAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using PharmEtrade_ApiGateway.Repository.Interface;
using System.Data;

namespace PharmEtrade_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductFilterController : ControllerBase
    {
        private readonly IProductFilterRepo _iproductFilterRepo;
        public ProductFilterController(IProductFilterRepo iproductFilterRepo)
        {
            _iproductFilterRepo = iproductFilterRepo;
        }
        // Author: [mamatha]
        // Created Date: [02/07/2024]
        // Description: Method for GetFilteredProducts
        [HttpGet]
        [Route("GetFilteredProducts")]
        public async Task<IActionResult> GetFilteredProducts(int? productCategoryId, string productName)
        {
            var products = await _iproductFilterRepo.GetFilteredProducts(productCategoryId, productName);

            if (products == null || products.Count == 0)
            {
                return NotFound();
            }

            return Ok(products);
        }
        // Author: [mamatha]
        // Created Date: [02/07/2024]
        // Description: Method for GetAllProducts
        [HttpGet]
        [Route("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(await _iproductFilterRepo.GetProducts());
        }


    }
}
