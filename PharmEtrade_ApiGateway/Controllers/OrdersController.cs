using BAL.RequestModels;
using BAL.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmEtrade_ApiGateway.Repository.Interface;
using BAL.Models;
using System.IO;

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

        [HttpPost("Place")]
        public async Task<IActionResult> AddUpdateOrder(OrderRequest request)
        {
            OrderResponse response = await _ordersRepository.AddUpdateOrder(request);
            return Ok(response);
        }

        [HttpGet("DownloadInvoice")]
        public async Task<IActionResult> DownInvoice(string orderId)
        {
            var invoiceStream = await _ordersRepository.DownloadInvoice(orderId);
            return File(invoiceStream.GetBuffer(), "application/pdf", "Invoice_" + orderId + ".pdf");
        }

        [HttpGet("DownloadInvoiceHtml")]
        public async Task<IActionResult> DownloadInvoiceHtml(string orderId)
        {
            var invoiceStream = await _ordersRepository.DownloadInvoiceHtml(orderId);
            return Ok(invoiceStream);
        }

        [HttpGet("SendInvoice")]
        public async Task<IActionResult> SendInvoice(string orderId)
        {
            var response = await _ordersRepository.SendInvoiceByMail(orderId);
            return Ok(response);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetOrdersByOrderId(string orderid)
        {
            var response = await _ordersRepository.GetOrdersByOrderId(orderid);
            return Ok(response);
        }

        [HttpPost("GetAllByDate")]
        public async Task<IActionResult> GetSellerOrdersByDate(OrderCriteria orderCriteria)
        {
            var response = await _ordersRepository.GetOrdersByDate(orderCriteria);
            return Ok(response);
        }

        [HttpGet("Buyer/GetAll")]
        public async Task<IActionResult> GetOrdersByCustomerId(string? customerId)
        {
            var response = await _ordersRepository.GetOrdersByCustomerId(customerId);
            return Ok(response);
        }

        [HttpPost("Buyer/GetAllByDate")]
        public async Task<IActionResult> GetCustomerOrdersByDate(BuyerOrderCriteria orderCriteria)
        {
            var response = await _ordersRepository.GetCustomerOrdersByDate(orderCriteria);
            return Ok(response);
        }

        [HttpGet("Buyer/Products")]
        public async Task<IActionResult> GetProductsByCustomerId(string? customerId)
        {
            var response = await _ordersRepository.GetOrdersByCustomerId(customerId);
            return Ok(response);
        }       

        // Seller Endpoints
        [HttpGet("Seller/GetAll")]
        public async Task<IActionResult> GetOrdersBySellerId(string? vendorId)
        {
            var response = await _ordersRepository.GetOrdersBySellerId(vendorId);
            return Ok(response);
        }
        
        [HttpPost("Seller/GetAllByDate")]
        public async Task<IActionResult> GetSellerOrdersByDate(SellerOrderCriteria orderCriteria)
        {
            var response = await _ordersRepository.GetSellerOrdersByDate(orderCriteria);
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

        [HttpGet("Seller/Payments")]
        public async Task<IActionResult> GetPaymentInfoBySellerId(string sellerId)
        {
            var response = await _paymentinfoRepository.GetPaymentInfoByCustmoerId(sellerId);
            return Ok(response);
        }

        [HttpPost("Payment")]
        public async Task<IActionResult> AddPayMent(PaymentRequest request)
        {
            PaymentResponse response = await _ordersRepository.AddPayment(request);
            return Ok(response);
        }

        [HttpPost("AddPayment")]
        public async Task<IActionResult> AddPaymentInfo(PaymentInfo paymentInfo)
        {
            var response = await _paymentinfoRepository.AddPayment(paymentInfo);
            return Ok(response);
        }

        [HttpPost("UpdatePayment")]
        public async Task<IActionResult> UpdatePaymentInfo(PaymentInfo paymentInfo)
        {
            var response = await _paymentinfoRepository.UpdatePayment(paymentInfo);
            return Ok(response);
        }

        [HttpGet("GetPaymentByOrderId")]
        public async Task<IActionResult> GetPaymentInfoByOrderId(string OrderId)
        {
            var response = await _paymentinfoRepository.GetPaymentInfoByOrderId(OrderId);
            return Ok(response);
        }

        [HttpGet("GetPaymentByCustomerId")]
        public async Task<IActionResult> GetPaymentInfoByCustomerId(string CustomerId)
        {
            var response = await _paymentinfoRepository.GetPaymentInfoByCustmoerId(CustomerId);
            return Ok(response);
        }

        [HttpGet("GetAllPayments")]
        public async Task<IActionResult> GetAllPayments()
        {
            var response = await _paymentinfoRepository.GetAllPayments();
            return Ok(response);
        }

        [HttpPost("GetPaymentsByDate")]
        public async Task<IActionResult> GetPaymentsByDate(PaymentCriteria criteria)
        {
            var response = await _paymentinfoRepository.GetAllPayments(criteria);
            return Ok(response);
        }
    }
}
