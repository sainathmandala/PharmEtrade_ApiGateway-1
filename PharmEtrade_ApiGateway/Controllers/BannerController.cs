using BAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmEtrade_ApiGateway.Repository.Interface;

namespace PharmEtrade_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BannerController : ControllerBase
    {
        private IBannerRepository bannerRepository;

        public BannerController(IBannerRepository bannerRepository) { 
            this.bannerRepository = bannerRepository;
        }

        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll() {
            var response = await bannerRepository.GetBanners();
            return Ok(response);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddBanner(Banner banner)
        {
            var response = await bannerRepository.AddUpdateBanner(banner);
            return Ok(response);
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> EditBanner(Banner banner)
        {
            var response = await bannerRepository.AddUpdateBanner(banner);
            return Ok(response);
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteBanner(int bannerId)
        {
            if(bannerId == 0)
            {
                return BadRequest("bannerId should not be null or zero.");
            }
            var response = await bannerRepository.DeleteBanner(bannerId);
            return Ok(response);
        }
    }
}
