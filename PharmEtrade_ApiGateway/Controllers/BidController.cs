using BAL.Models;
using BAL.RequestModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmEtrade_ApiGateway.Repository.Interface;
using PharmEtrade_ApiGateway.Repository.Helper;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;


namespace PharmEtrade_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BidController : ControllerBase
    {
        private readonly IBidRepository _ibidrepository;

        public BidController(IBidRepository bidRepository)
        {
            _ibidrepository = bidRepository;
        }
        [HttpGet("GetBidsByBuyer")]
        public async Task<IActionResult> GetBidsByBuyer(string buyerId)
        {
            if (string.IsNullOrEmpty(buyerId))
            {
                return BadRequest("buyerId is required.");
            }
            var response = await _ibidrepository.GetBidsByBuyer(buyerId);
            return Ok(response);

            
        }

        [HttpGet("GetBidsBySeller")]
        public async Task<IActionResult> GetBidsBySeller(string sellerId)
        {
            if (string.IsNullOrEmpty(sellerId))
            {
                return BadRequest("sellerId is required.");
            }
            var response = await _ibidrepository.GetBidsBySeller(sellerId);
            return Ok(response);
        }

        [HttpGet("GetBidsByProduct")]
        public async Task<IActionResult> GetBidsByProduct(string productId)
        {
            if (string.IsNullOrEmpty(productId))
            {
                return BadRequest("productId is required.");
            }
            var response = await _ibidrepository.GetBidsByProduct(productId);
            return Ok(response);
        }

        [HttpGet("GetProductsQuotedByBuyer")]
        public async Task<IActionResult> GetProductsQuotesByBuyer(string BuyerId)
        {
            if (string.IsNullOrEmpty(BuyerId))
            {
                return BadRequest("sellerId is required.");
            }
            var response = await _ibidrepository.GetProductsQuotesByBuyer(BuyerId);
            return Ok(response);
        }

        [HttpGet("GetProductsQuotedBySeller")]
        public async Task<IActionResult> GetProductsQuotesBySeller(string sellerId)
        {
            if (string.IsNullOrEmpty(sellerId))
            {
                return BadRequest("sellerId is required.");
            }
            var response = await _ibidrepository.GetProductsQuotesBySeller(sellerId);
            return Ok(response);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddBid(Bids bid)
        {
            var response = await _ibidrepository.AddBid(bid);
            return Ok(response);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateBid(Bids bid)
        {
            var response = await _ibidrepository.UpdateBid(bid);
            return Ok(response);
        }

        [HttpPost("Remove")]
        public async Task<IActionResult> RemoveBid(string bidId)
        {
            var response = await _ibidrepository.RemoveBid(bidId);
            return Ok(response);
        }
    }
}
