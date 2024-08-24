using BAL.RequestModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmEtrade_ApiGateway.Repository.Interface;

namespace PharmEtrade_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private IWishListRepository _wishListRepository;
        public WishListController(IWishListRepository WishListRepository)
        {
            _wishListRepository = WishListRepository;
        }

        [HttpGet]
        [Route("GetWishListItems")]
        public async Task<IActionResult> GetWishListItems(string? customerId = null)
        {
            var response = await _wishListRepository.GetWishListItems(customerId);
            return Ok(response);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddToWishList(WishListRequest request)
        {
            var response = await _wishListRepository.AddToWishList(request);
            return Ok(response);
        }
        [HttpGet]
        [Route("GetWishListById")]
        public async Task<IActionResult> GetWishListById(string? WishListId = null)
        {
            var response = await _wishListRepository.GetWishListById(WishListId);
            return Ok(response);
        }
    }
}
