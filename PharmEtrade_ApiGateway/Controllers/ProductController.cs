using BAL.Models;
using BAL.RequestModels;
using BAL.ResponseModels;
using BAL.ViewModel;
using BAL.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmEtrade_ApiGateway.Repository.Helper;
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

        [HttpGet("GetProductOffers")]
        public async Task<IActionResult> GetProductOffers(int specificationId)
        {
            var response = await _productRepo.GetProductsBySpecification(specificationId,true);
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

        [HttpGet("SpecialOffers")]
        public async Task<IActionResult> GetSpecialOffers()
        {
            var response = await _productRepo.GetSpecialOffers();
            return Ok(response);
        } 
        

        [HttpGet("GetRelatedProducts")]
        public async Task<IActionResult> GetRelatedProducts(string productId)
        {
            var response = await _productRepo.GetRelatedProducts(productId);
            return Ok(response);
        }

        [HttpGet("GetUpsellProducts")]
        public async Task<IActionResult> GetUpsellProducts(string productId)
        {
            var response = await _productRepo.GetUpsellProducts(productId);
            return Ok(response);
        }

        [HttpGet("GetCrossSellProducts")]
        public async Task<IActionResult> GetCrossSellProducts(string productId)
        {
            var response = await _productRepo.GetCrossSellProducts(productId);
            return Ok(response);
        }

        [HttpPost("AddRelatedProduct")]
        public async Task<IActionResult> AddRelatedProduct(string productId, string relatedProductId)
        {
            var response = await _productRepo.AddRelatedProduct(productId, relatedProductId);
            return Ok(response);
        }

        [HttpPost("AddUpsellProduct")]
        public async Task<IActionResult> AddUpsellProduct(string productId, string upsellProductId)
        {
            var response = await _productRepo.AddUpsellProduct(productId, upsellProductId);
            return Ok(response);
        }

        [HttpPost("AddCrossSellProduct")]
        public async Task<IActionResult> AddCrossSellProduct(string productId, string crossSellProductId)
        {
            var response = await _productRepo.AddCrossSellProduct(productId, crossSellProductId);
            return Ok(response);
        }

        [HttpPost("RemoveRelatedProduct")]
        public async Task<IActionResult> RemoveRelatedProduct(string productId, string relatedProductId)
        {
            var response = await _productRepo.RemoveRelatedProduct(productId, relatedProductId);
            return Ok(response);
        }

        [HttpPost("RemoveUpsellProduct")]
        public async Task<IActionResult> RemoveUpsellProduct(string productId, string upsellProductId)
        {
            var response = await _productRepo.RemoveUpsellProduct(productId, upsellProductId);
            return Ok(response);
        }

        [HttpPost("RemoveCrossSellProduct")]
        public async Task<IActionResult> RemoveCrossSellProduct(string productId, string crossSellProductId)
        {
            var response = await _productRepo.RemoveCrossSellProduct(productId, crossSellProductId);
            return Ok(response);
        }
        [HttpPost("AddRating")]
        public async Task<IActionResult> AddProductRating(ProductRating productRating)
        {
            var response = await _productRepo.AddProductRating(productRating);
            return Ok(response);
        }
        [HttpPost("UpdateRating")]
        public async Task<IActionResult> UpdateProductRating(ProductRating productRating)
        {
            var response = await _productRepo.UpdateProductRating(productRating);
            return Ok(response);
        }
        [HttpGet("GetRatingWithProduct")]
        public async Task<IActionResult> GetRatingwithProduct(string productId)
        {
            var response = await _productRepo.GetRatingwithProduct(productId);
            return Ok(response);
        }
        [HttpGet("GetRatingById")]
        public async Task<IActionResult> GetRating(string RatingID)
        {
            var response = await _productRepo.GetRating(RatingID);
            return Ok(response);
        }
        [HttpPost("RemoveProductRating")]
        public async Task<IActionResult> RemoveProductRating(string RatingID)
        {
            var response = await _productRepo.RemoveProductRating(RatingID);
            return Ok(response);
        }
    }
}