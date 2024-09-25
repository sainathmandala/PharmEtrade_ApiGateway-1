using BAL.BusinessLogic.Interface;
using BAL.Models;
using BAL.ResponseModels;
using PharmEtrade_ApiGateway.Repository.Interface;

namespace PharmEtrade_ApiGateway.Repository.Helper
{
    public class MastersRepository : IMastersRepository
    {
        private readonly IMastersHelper _mastersHelper;
        public MastersRepository(IMastersHelper mastersHelper)
        {
            _mastersHelper = mastersHelper;
        }

        public async Task<Response<CategorySpecification>> AddCategorySpecification(CategorySpecification categorySpecification)
        {
            return await _mastersHelper.AddCategorySpecification(categorySpecification);
        }

        public async Task<Response<ProductCategory>> AddProductCategory(ProductCategory productCategory)
        {
            return await _mastersHelper.AddProductCategory(productCategory);
        }

        public async Task<Response<CategorySpecification>> GetCategorySpecifications(int categorySpecificationId = 0)
        {
            return await _mastersHelper.GetCategoriesSpecification(categorySpecificationId);
        }

        public async Task<Response<NDCUPC>> GetNDCUPCDetails(string? NDC, string? UPC)
        {
            return await _mastersHelper.GetNDCUPCDetails(NDC, UPC);
        }

        public async Task<Response<ProductCategory>> GetProductCategories(int categoryId = 0)
        {
            return await _mastersHelper.GetProductCategories(categoryId);
        }

        public async Task<Response<CategorySpecification>> RemoveCategorySpecification(int categorySpecificationId)
        {
            return await _mastersHelper.RemoveCategorySpecification(categorySpecificationId);
        }

        public async Task<Response<ProductCategory>> RemoveProductCategory(int categoryId)
        {
            return await _mastersHelper.RemoveProductCategory(categoryId);
        }
    }
}
