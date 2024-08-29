using BAL.Models;
using BAL.ResponseModels;

namespace PharmEtrade_ApiGateway.Repository.Interface
{
    public interface IMenuRepository
    {
        Task<Response<Menu>> GetMenuByAccountType(string CustomerTypeId);
    }
}
