using BAL.Models;
using BAL.RequestModels;
using BAL.ResponseModels;
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

        [HttpPost("Add")]
        public async Task<IActionResult> AddProduct(Product product)
        {
            var response = await _productRepo.AddProduct(product);
            return Ok(response);
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> EditProduct(Product product)
        {
            if (string.IsNullOrEmpty(product.ProductID))
            {
                return BadRequest("Product Id is required.");
            }
            var response = await _productRepo.AddProduct(product);
            return Ok(response);
        }

        [HttpPost]
        [Route("Image/Upload")]
        public async Task<IActionResult> UploadImage(IFormFile image, string sellerId)
        {
            if(string.IsNullOrEmpty(sellerId) || image == null)
            {
                return BadRequest("Image and Seller Id are required.");
            }
            UploadResponse response = await _productRepo.UploadImage(image, sellerId);
            return Ok(response);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllProducts()
        {
            var response = await _productRepo.GetAllProducts();
            return Ok(response);
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetProductsById(string productId)
        {
            if (string.IsNullOrEmpty(productId))
            {
                return BadRequest("Product Id is required.");
            }
            var response = await _productRepo.GetAllProducts(productId);
            return Ok(response);
        }
        
        [HttpGet("GetRxProducts")]
        public async Task<IActionResult> GetRxProducts()
        {
            var response = await _productRepo.GetProductsBySpecification(4);
            return Ok(response);
        }

        [HttpGet("GetOTCProducts")]
        public async Task<IActionResult> GetOTCProducts()
        {
            var response = await _productRepo.GetProductsBySpecification(3);
            return Ok(response);
        }

        [HttpGet("GetRecentSoldProducts")]
        public async Task<IActionResult> GetRecentSoldProducts(int? numberOfProducts)
        {
            if (numberOfProducts == null)
                numberOfProducts = 10;
            var response = await _productRepo.GetRecentSoldProducts(numberOfProducts.Value);
            return Ok(response);
        }

        [HttpPost("Size/Add")]
        public async Task<IActionResult> AddProductSize(ProductSize productSize)
        {
            var response = await _productRepo.AddUpdateProductSize(productSize);
            return Ok(response);
        }

        [HttpPost("Size/Edit")]
        public async Task<IActionResult> EditProductSize(ProductSize productSize)
        {
            var response = await _productRepo.AddUpdateProductSize(productSize);
            return Ok(response);
        }
    }
}