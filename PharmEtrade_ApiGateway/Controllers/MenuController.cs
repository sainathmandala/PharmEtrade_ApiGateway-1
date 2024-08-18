using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmEtrade_ApiGateway.Repository.Interface;

namespace PharmEtrade_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private IMenuRepository _menuRepository;
        public MenuController(IMenuRepository menuRepository) {
            _menuRepository = menuRepository;
        }

        [HttpGet]
        [Route("GetByAccountType")]
        public async Task<IActionResult> GetMenuByAccountType(int accountTypeId)
        {
            return Ok(await _menuRepository.GetMenuByAccountType(accountTypeId));
        }
    }
}
