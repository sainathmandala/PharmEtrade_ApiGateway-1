using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmEtrade_ApiGateway.Repository.Interface;

namespace PharmEtrade_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MastersController : ControllerBase
    {
        private readonly IMastersRepository mastersRepository;

        public MastersController(IMastersRepository mastersRepository)
        {
            this.mastersRepository = mastersRepository;
        }

        [HttpGet("GetNDCUPCList")]
        public async Task<IActionResult> GetNDCUPCDetails(string? NDC, string? UPC)
        {
            if (string.IsNullOrEmpty(NDC) && string.IsNullOrEmpty(UPC))
            {
                return BadRequest("Either NDC or UPC required.");
            }
            var response = await mastersRepository.GetNDCUPCDetails(NDC, UPC);
            return Ok(response);
        }
    }
}
