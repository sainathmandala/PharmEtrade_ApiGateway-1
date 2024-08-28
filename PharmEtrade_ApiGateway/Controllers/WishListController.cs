using BAL.RequestModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmEtrade_ApiGateway.Repository.Helper;
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
        [HttpPost]
        [Route("Remove")]
        public async Task<IActionResult>Remove(string? wishlistId)
        {
            if (string.IsNullOrEmpty(wishlistId))
            {
                return BadRequest("CartId should not be null or empty.");
            }
            var response = await _wishListRepository.RemoveWishList(wishlistId);
            return Ok(response);
        }
    }
}
