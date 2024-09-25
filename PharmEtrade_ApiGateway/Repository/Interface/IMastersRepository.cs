using BAL.Models;
using BAL.ResponseModels;

namespace PharmEtrade_ApiGateway.Repository.Interface
{
    public interface IMastersRepository
    {
        Task<Response<NDCUPC>> GetNDCUPCDetails(string? NDC, string? UPC);
        Task<Response<ProductCategory>> GetProductCategories(int categoryId = 0);
        Task<Response<ProductCategory>> AddProductCategory(ProductCategory productCategory);
        Task<Response<ProductCategory>> RemoveProductCategory(int categoryId);

        Task<Response<CategorySpecification>> GetCategorySpecifications(int categorySpecificationId = 0);
        Task<Response<CategorySpecification>> AddCategorySpecification(CategorySpecification categorySpecification);
        Task<Response<CategorySpecification>> RemoveCategorySpecification(int categorySpecificationId);

    }
}
