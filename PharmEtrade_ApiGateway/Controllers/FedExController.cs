using BAL.Models.FedEx.RateRequest;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using PharmEtrade_ApiGateway.Repository.Interface;
using System.Text.Json;
using System.Text;

namespace PharmEtrade_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FedExController : ControllerBase
    {
        private readonly IFedExRepository _fedexRepository;
        public FedExController(IFedExRepository fedExRepository) { 
            _fedexRepository = fedExRepository;
        }

        [HttpGet("track")]
        public async Task<ActionResult> GetTrackingInfo(string trackingNumber)
        {
            var response = await _fedexRepository.GetTrackingInfo(trackingNumber);
            if(response.Status.StartsWith("400") || response.Status.StartsWith("500"))
                return BadRequest(response.Status.Split("::")[1]);
            if (response.Status.StartsWith("401"))
                return Unauthorized(response.Status.Split("::")[1]);            
            return Ok(response);
        }

        [HttpPost("Rates")]
        public async Task<ActionResult> GetRates(RateRequest request)
        {
            var response = await _fedexRepository.GetRates(request);
            return Ok(response);
        }

        [HttpPost("ServiceTypes")]
        public async Task<ActionResult> GetServiceTypes(RateRequest request)
        {
            var response = await _fedexRepository.GetServiceTypes(request);
            return Ok(response);
        }
    }
}
