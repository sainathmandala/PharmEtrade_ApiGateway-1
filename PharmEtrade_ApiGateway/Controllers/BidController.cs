using BAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmEtrade_ApiGateway.Repository.Interface;

namespace PharmEtrade_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {

        [HttpGet("GetBidsByBuyer")]
        public async Task<IActionResult> GetBidsByBuyer(string buyerId)
        {
            var response = ""; 
            return Ok(response);
        }

        [HttpGet("GetBidsBySeller")]
        public async Task<IActionResult> GetBidsBySeller(string sellerId)
        {
            var response = "";
            return Ok(response);
        }

        [HttpGet("GetBidsByProduct")]
        public async Task<IActionResult> GetBidsByProduct(string productId)
        {
            var response = "";
            return Ok(response);
        }

        [HttpGet("GetProductsQuotedByBuyer")]
        public async Task<IActionResult> GetProductsQuotesByBuyer(string sellerId)
        {
            var response = "";
            return Ok(response);
        }

        [HttpGet("GetProductsQuotedBySeller")]
        public async Task<IActionResult> GetProductsQuotesBySeller(string sellerId)
        {
            var response = "";
            return Ok(response);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddBid(Bid bid)
        {
            var response = "";
            return Ok(response);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateBid(Bid bid)
        {
            var response = "";
            return Ok(response);
        }

        [HttpPost("Remove")]
        public async Task<IActionResult> RemoveBid(string bidId)
        {
            var response = "";
            return Ok(response);
        }
    }
}
