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
        private IPaymentinfoRepository _paymentinfoRepository;
        public OrdersController(IOrdersRepository ordersRepository,IPaymentinfoRepository paymentinfoRepository) 
        { 
            _ordersRepository = ordersRepository;
            _paymentinfoRepository = paymentinfoRepository;
        }

        //[HttpPost]
        //[Route("Add")]
        //public async Task<IActionResult> AddOrder(TempOrderRequest request)
        //{
        //    OrderResponse response = await _ordersRepository.AddOrder(request);
        //    return Ok(response);
        //}

        [HttpPost]
        [Route("Place")]
        public async Task<IActionResult> AddUpdateOrder(OrderRequest request)
        {
            OrderResponse response = await _ordersRepository.AddUpdateOrder(request);
            return Ok(response);
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetOrdersByOrderId(string orderid)
        {
            var response = await _ordersRepository.GetOrdersByOrderId(orderid);
            return Ok(response);
        }

        [HttpGet]
        [Route("Buyer/GetAll")]
        public async Task<IActionResult> GetOrdersByCustomerId(string? customerId)
        {
            var response = await _ordersRepository.GetOrdersByCustomerId(customerId);
            return Ok(response);
        }

        // Seller Endpoints
        [HttpGet]
        [Route("Seller/GetAll")]
        public async Task<IActionResult> GetOrdersBySellerId(string? vendorId)
        {
            var response = await _ordersRepository.GetOrdersBySellerId(vendorId);
            return Ok(response);
        }

        [HttpGet("Seller/Products")]
        public async Task<IActionResult> GetProductsOrderedForSeller(string vendorId)
        {
            var response = await _ordersRepository.GetOrdersBySellerId(vendorId);
            return Ok(response);
        }

        [HttpGet("Seller/Customers")]
        public async Task<IActionResult> GetCustomersOrderedForSeller(string vendorId)
        {
            var response = await _ordersRepository.GetCustomersOrderedForSeller(vendorId);
            return Ok(response);
        }

        [HttpGet]
        [Route("Seller/Payments")]
        public async Task<IActionResult> GetPaymentInfoBySellerId(string sellerId)
        {
            var response = await _paymentinfoRepository.GetPaymentInfoByCustmoerId(sellerId);
            return Ok(response);
        }

        [HttpPost]
        [Route("Payment")]
        public async Task<IActionResult> AddPayMent(PaymentRequest request)
        {
            PaymentResponse response = await _ordersRepository.AddPayment(request);
            return Ok(response);
        }
        [HttpPost]
        [Route("AddPayment")]
        public async Task<IActionResult> AddPaymentInfo(PaymentInfo paymentInfo)
        {
            var response = await _paymentinfoRepository.AddPayment(paymentInfo);
            return Ok(response);
        }
        [HttpPost]
        [Route("UpdatePayment")]
        public async Task<IActionResult> UpdatePaymentInfo(PaymentInfo paymentInfo)
        {
            var response = await _paymentinfoRepository.UpdatePayment(paymentInfo);
            return Ok(response);
        }
        [HttpGet]
        [Route("GetPaymentByOrderId")]
        public async Task<IActionResult> GetPaymentInfoByOrderId(string OrderId)
        {
            var response = await _paymentinfoRepository.GetPaymentInfoByOrderId(OrderId);
            return Ok(response);
        }

        [HttpGet]
        [Route("GetPaymentByCustomerId")]
        public async Task<IActionResult> GetPaymentInfoByCustomerId(string CustomerId)
        {
            var response = await _paymentinfoRepository.GetPaymentInfoByCustmoerId(CustomerId);
            return Ok(response);
        }

    }
}
