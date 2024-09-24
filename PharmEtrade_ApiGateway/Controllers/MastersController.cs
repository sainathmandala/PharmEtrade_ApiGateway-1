using BAL.Models;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PharmEtrade_ApiGateway.Repository.Interface;

namespace PharmEtrade_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MastersController : ControllerBase
    {
        private readonly IMastersRepository mastersRepository;

        public MastersController(IMastersRepository mastersRepository)
        {
            this.mastersRepository = mastersRepository;
        }

        [HttpGet("GetNDCUPCList")]
        public async Task<IActionResult> GetNDCUPCDetails(string? NDC, string? UPC)
        {
            if (string.IsNullOrEmpty(NDC) && string.IsNullOrEmpty(UPC))
            {
                return BadRequest("Either NDC or UPC required.");
            }
            var response = await mastersRepository.GetNDCUPCDetails(NDC, UPC);
            return Ok(response);
        }

        [HttpGet("ProductCategories/GetAll")]
        public async Task<IActionResult> GetProductCategories()
        {            
            var response = await mastersRepository.GetProductCategories();
            return Ok(response);
        }

        [HttpGet("ProductCategories/GetById")]
        public async Task<IActionResult> GetProductCategoriesById(int categoryId)
        {
            if(categoryId == 0) {
                return BadRequest("Product Category Id should not be zero.");
            }
            var response = await mastersRepository.GetProductCategories(categoryId);
            return Ok(response);
        }

        [HttpPost("ProductCategories/Add")]
        public async Task<IActionResult> AddProductCategoriesById(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                return BadRequest("Product Category Name is required.");
            }
            var response = await mastersRepository.AddProductCategory(new ProductCategory() { ProductCategoryId = 0, CategoryName = categoryName });
            return Ok(response);
        }

        [HttpPost("ProductCategories/Update")]
        public async Task<IActionResult> AddProductCategoriesById(ProductCategory productCategory)
        {
            if (productCategory.ProductCategoryId == 0)
            {
                return BadRequest("Product Category Id should not be zero.");
            }
            if (string.IsNullOrEmpty(productCategory.CategoryName))
            {
                return BadRequest("Product Category Name is required.");
            }
            var response = await mastersRepository.AddProductCategory(productCategory);
            return Ok(response);
        }

        [HttpGet("ProductCategories/Remove")]
        public async Task<IActionResult> RemoveProductCategoriesById(int categoryId)
        {
            if (categoryId == 0)
            {
                return BadRequest("Product Category Id should not be zero.");
            }
            var response = await mastersRepository.RemoveProductCategory(categoryId);
            return Ok(response);
        }

        [HttpGet("CategorySpecifications/GetAll")]
        public async Task<IActionResult> GetCategorySpecificationsById()
        {
            var response = await mastersRepository.GetCategorySpecifications();
            return Ok(response);
        }
        [HttpGet("CategorySpecifications/GetById")]
        public async Task<IActionResult> GetCategorySpecificationsById(int categorySpecificationId)
        {
            if (categorySpecificationId == 0)
            {
                return BadRequest("Product Category Id should not be zero.");
            }
            var response = await mastersRepository.GetCategorySpecifications(categorySpecificationId);
            return Ok(response);
        }

        [HttpPost("CategorySpecifications/Add")]
        public async Task<IActionResult> AddCategorySpecificationsById(string specificationName)
        {
            if (string.IsNullOrEmpty(specificationName))
            {
                return BadRequest("Category Specification Name is required.");
            }
            var response = await mastersRepository.AddCategorySpecification(new CategorySpecification() { CategorySpecificationId = 0, SpecificationName = specificationName });
            return Ok(response);
        }

        [HttpPost("CategorySpecifications/Update")]
        public async Task<IActionResult> AddCategorySpecificationsById(CategorySpecification categorySpecification)
        {
            if (categorySpecification.CategorySpecificationId == 0)
            {
                return BadRequest("Category Specification Id should not be zero.");
            }
            if (string.IsNullOrEmpty(categorySpecification.SpecificationName))
            {
                return BadRequest("Category Specification Name is required.");
            }
            var response = await mastersRepository.AddCategorySpecification(categorySpecification);
            return Ok(response);
        }

        [HttpGet("CategorySpecifications/Remove")]
        public async Task<IActionResult> RemoveCategorySpecificationsById(int categorySpecificationId)
        {
            if (categorySpecificationId == 0)
            {
                return BadRequest("Category Specification Id should not be zero.");
            }
            var response = await mastersRepository.RemoveCategorySpecification(categorySpecificationId);
            return Ok(response);
        }
    }
}
