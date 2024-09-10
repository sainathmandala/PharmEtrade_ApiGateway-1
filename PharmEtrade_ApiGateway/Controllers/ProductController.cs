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

        [HttpPost("ProductInfo/Add")]
        public async Task<IActionResult> AddProductInfo(ProductInfo productInfo)
        {
            var response = await _productRepo.AddUpdateProductInfo(productInfo);
            return Ok(response);
        }

        [HttpPost("ProductInfo/Edit")]
        public async Task<IActionResult> EditProductInfo(ProductInfo productInfo)
        {
            var response = await _productRepo.AddUpdateProductInfo(productInfo);
            return Ok(response);
        }

        [HttpPost("Price/Add")]
        public async Task<IActionResult> AddProductPriceDetails(ProductPrice productPrice)
        {
            var response = await _productRepo.AddUpdateProductPrice(productPrice);
            return Ok(response);
        }

        [HttpPost("Price/Edit")]
        public async Task<IActionResult> EditProductPriceDetails(ProductPrice productPrice)
        {
            var response = await _productRepo.AddUpdateProductPrice(productPrice);
            return Ok(response);
        }

        [HttpPost("Gallery/Add")]
        public async Task<IActionResult> AddProductGallery(ProductGallery productgallery)
        {
            var response = await _productRepo.AddUpdateProductGallery(productgallery);
            return Ok(response);
        }

        [HttpPost("Gallery/Edit")]
        public async Task<IActionResult> EditProductGallery(ProductGallery productgallery)
        {
            var response = await _productRepo.AddUpdateProductGallery(productgallery);
            return Ok(response);
        }

        [HttpPost]
        [Route("Image/Upload")]
        public async Task<IActionResult> UploadImage(IFormFile image, string sellerId, string productId)
        {
            if (string.IsNullOrEmpty(sellerId) || image == null)
            {
                return BadRequest("Image and Seller Id are required.");
            }
            UploadResponse response = await _productRepo.UploadImage(image, sellerId, productId);
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
            var response = await _productRepo.GetProductsBySpecification(1);
            return Ok(response);
        }

        [HttpGet("GetOTCProducts")]
        public async Task<IActionResult> GetOTCProducts()
        {
            var response = await _productRepo.GetProductsBySpecification(2);
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

        [HttpGet("GetBySeller")]
        public async Task<IActionResult> GetProductsBySeller(string sellerId)
        {
            if (string.IsNullOrEmpty(sellerId))
            {
                return BadRequest("Seller Id is required.");
            }
            var response = await _productRepo.GetProductsBySeller(sellerId);
            return Ok(response);
        }

        [HttpPost("GetByCriteria")]
        public async Task<IActionResult> GetProductsByCriteria(ProductCriteria criteria)
        {
            var response = await _productRepo.GetProductsByCriteria(criteria);
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