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

        Task<Response<ProductCategory>> GetCategorySpecifications(int categorySpecificationId = 0);
        Task<Response<ProductCategory>> AddCategorySpecification(CategorySpecification categorySpecification);
        Task<Response<ProductCategory>> RemoveCategorySpecification(int categorySpecificationId);

    }
}
