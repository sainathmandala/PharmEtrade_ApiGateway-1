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

        public async Task<Response<Menu>> GetMenuByAccountType(int accountTypeId = 0)
        {
            return await _menuHelper.GetMenuByAccountType(accountTypeId);
        }
    }
}
