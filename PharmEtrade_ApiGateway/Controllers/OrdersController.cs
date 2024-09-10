using BAL.RequestModels;
using BAL.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmEtrade_ApiGateway.Repository.Interface;
using BAL.Models;

namespace PharmEtrade_ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private IOrdersRepository _ordersRepository;
        public OrdersController(IOrdersRepository ordersRepository) 
        { 
            _ordersRepository = ordersRepository;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddOrder(OrderRequest request)
        {
            OrderResponse response = await _ordersRepository.AddOrder(request);
            return Ok(response);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> GetOrdersByCustomerId(string? customerId)
        {
            var response = await _ordersRepository.GetOrdersByCustomerId(customerId);
            return Ok(response);
        }
        [HttpGet]
        [Route("GetOrdersBySellerId")]
        public async Task<IActionResult> GetOrdersBySellerId(string? vendorId)
        {
            var response = await _ordersRepository.GetOrdersBySellerId(vendorId);
            return Ok(response);
        }
        [HttpPost]
        [Route("Payment")]
        public async Task<IActionResult> AddPayMent(PaymentRequest request)
        {
            PaymentResponse response = await _ordersRepository.AddPayment(request);
            return Ok(response);
        }

       
    }
}
