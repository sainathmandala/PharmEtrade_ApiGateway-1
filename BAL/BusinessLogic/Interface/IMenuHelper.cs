using BAL.Models;
using BAL.ResponseModels;

namespace BAL.BusinessLogic.Interface
{
    public interface IMenuHelper
    {
        Task<Response<Menu>> GetMenuByAccountType(string CustomerTypeId);
    }
}
