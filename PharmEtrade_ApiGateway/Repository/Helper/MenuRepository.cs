using BAL.BusinessLogic.Interface;
using BAL.Models;
using BAL.ResponseModels;
using PharmEtrade_ApiGateway.Repository.Interface;

namespace PharmEtrade_ApiGateway.Repository.Helper
{
    public class MenuRepository : IMenuRepository
    {
        private IMenuHelper _menuHelper;
        public MenuRepository(IMenuHelper menuHelper) { 
            _menuHelper = menuHelper;
        }

        public async Task<Response<Menu>> GetMenuByAccountType(string CustomerTypeId)
        {
            return await _menuHelper.GetMenuByAccountType(CustomerTypeId);
        }
    }
}
