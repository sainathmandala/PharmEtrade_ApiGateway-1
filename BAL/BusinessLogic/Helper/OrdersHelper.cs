using BAL.BusinessLogic.Interface;
using BAL.Common;
using BAL.RequestModels;
using BAL.ResponseModels;
using DAL;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using DAL.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using PdfSharpCore;
using System.Net.Http;
using SelectPdf;
using BAL.ViewModels;
using Org.BouncyCastle.Crypto.Macs;
//using PdfSharpCore;
//using PdfSharpCore.Pdf;
//using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace BAL.BusinessLogic.Helper
{
    public class OrdersHelper : IOrders
    {
        private readonly IsqlDataHelper _isqlDataHelper;
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        private readonly IEmailHelper _emailHelper;

        public OrdersHelper(IConfiguration configuration, IsqlDataHelper isqlDataHelper, IEmailHelper emailHelper)
        {
            _configuration = configuration;
            _isqlDataHelper = isqlDataHelper;
            _connectionString = configuration.GetConnectionString("APIDBConnectionString");
            _emailHelper = emailHelper;
        }

        public async Task<OrderResponse> AddOrder(TempOrderRequest orderRequest)
        {
            OrderResponse response = new OrderResponse();
            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(StoredProcedures.ADD_UPDATE_ORDER, sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_OrderId", orderRequest.OrderId);
                    cmd.Parameters.AddWithValue("@p_CustomerId", orderRequest.CustomerId);
                    cmd.Parameters.AddWithValue("@p_ProductId", orderRequest.ProductId);
                    cmd.Parameters.AddWithValue("@p_Quantity", orderRequest.Quantity);
                    cmd.Parameters.AddWithValue("@p_PricePerProduct", orderRequest.PricePerProduct);
                    cmd.Parameters.AddWithValue("@p_TotalAmount", orderRequest.TotalAmount);
                    cmd.Parameters.AddWithValue("@p_ShippingMethodId", orderRequest.ShippingMethodId);
                    cmd.Parameters.AddWithValue("@p_OrderStatusId", orderRequest.OrderStatusId);
                    cmd.Parameters.AddWithValue("@p_VendorId", orderRequest.VendorId);
                    cmd.Parameters.AddWithValue("@p_TrackingNumber", orderRequest.TrackingNumber);
                    cmd.Parameters.AddWithValue("@p_ImageUrl", orderRequest.ImageUrl);

                    try
                    {
                        DataTable tblOrders = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmd));

                        if (tblOrders.Rows.Count > 0)
                        {
                            response.Status = 200;
                            response.CustomerName = tblOrders.Rows[0]["CustomerName"].ToString() ?? "";
                            //response.ProductName = tblOrders.Rows[0]["ProductName"].ToString() ?? "";
                            response.OrderId = tblOrders.Rows[0]["OrderId"].ToString() ?? "";
                            //response.ImageUrl = orderRequest.ImageUrl;
                            string _customerEmail = tblOrders.Rows[0]["CustomerEmail"].ToString() ?? "";
                            int _quantity = Convert.ToInt32(string.IsNullOrEmpty(tblOrders.Rows[0]["Quantity"].ToString() ?? "") ? 0 : tblOrders.Rows[0]["Quantity"]);

                            //response.VendorName = tblOrders.Rows[0][""].ToString();
                            response.Message = "Success";
                            //string _mailBody = string.Format(EmailTemplates.ORDER_TEMPLATE
                            //                                    , 1
                            //                                    , orderRequest.ImageUrl
                            //                                    , response.ProductName
                            //                                    , orderRequest.PricePerProduct
                            //                                    , _quantity
                            //                                    , (orderRequest.Quantity * orderRequest.PricePerProduct));
                            //_mailBody.Replace("[[OrderId]]", response.OrderId);
                            //await _emailHelper.SendEmail(_customerEmail, "", "Your Order Has been Placed", _mailBody);
                        }
                        else
                        {
                            response.Status = 400;
                            response.Message = "Failed to add product to cart.";
                        }
                    }
                    catch (MySqlException ex) when (ex.Number == 500)
                    {
                        //Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertAddToCartProduct : errormessage:" + ex.Message.ToString()));
                        response.Status = 500;
                        response.Message = "ERROR : " + ex.Message;
                    }
                    catch (Exception ex)
                    {
                        //Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertAddToCartProduct : errormessage:" + ex.Message.ToString()));
                        response.Status = 500;
                        response.Message = "ERROR : " + ex.Message;
                    }
                    return response;
                }
            }
        }

        public async Task<OrderResponse> AddUpdateOrder(OrderRequest orderRequest)
        {
            OrderResponse response = new OrderResponse();
            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(StoredProcedures.ADD_UPDATE_ORDER, sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_OrderId", orderRequest.OrderId);
                    cmd.Parameters.AddWithValue("@p_CustomerId", orderRequest.CustomerId);
                    cmd.Parameters.AddWithValue("@p_TotalAmount", orderRequest.TotalAmount);
                    cmd.Parameters.AddWithValue("@p_ShippingMethodId", orderRequest.ShippingMethodId);
                    cmd.Parameters.AddWithValue("@p_OrderStatusId", orderRequest.OrderStatusId);
                    cmd.Parameters.AddWithValue("@p_TrackingNumber", orderRequest.TrackingNumber);
                    var productsTable = JsonConvert.SerializeObject(orderRequest.Products);
                    cmd.Parameters.AddWithValue("@p_ProductsTable", productsTable);

                    try
                    {
                        DataTable tblOrders = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmd));

                        if (tblOrders.Rows.Count > 0)
                        {
                            response = MapDatatableToOrderResponse(tblOrders);
                            response.Status = 200;
                            response.Message = "Success";

                            string _mailBody = EmailTemplates.ORDER_TEMPLATE;
                            _mailBody = _mailBody.Replace("{{OrderId}}", response.OrderId);
                            _mailBody = _mailBody.Replace("{{OrderDetailsHTML}}", GetOrderDetailsHTML(response));                            
                            await _emailHelper.SendEmail(response.CustomerEmail, "", "Your Order Has been Placed", _mailBody);
                        }
                        else
                        {
                            response.Status = 400;
                            response.Message = "Failed to add product to cart.";
                        }
                    }
                    catch (MySqlException ex) when (ex.Number == 500)
                    {
                        //Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertAddToCartProduct : errormessage:" + ex.Message.ToString()));
                        response.Status = 500;
                        response.Message = "ERROR : " + ex.Message;
                    }
                    catch (Exception ex)
                    {
                        //Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertAddToCartProduct : errormessage:" + ex.Message.ToString()));
                        response.Status = 500;
                        response.Message = "ERROR : " + ex.Message;
                    }
                    return response;
                }
            }
        }

        private string GetOrderDetailsHTML(OrderResponse response)
        {
            string _orderDetailsHTML = "";
            int sNumber = 1;
            foreach (var details in response.Products)
            {
                _orderDetailsHTML += "<tr>";
                _orderDetailsHTML += string.Format("<td> {0} </td>", sNumber);
                _orderDetailsHTML += string.Format("<td> <img src='{0}' width='150px' height='100px' /> </td>", details.ImageUrl);
                _orderDetailsHTML += string.Format("<td> {0} </td>",details.ProductName);
                _orderDetailsHTML += string.Format("<td> {0} </td>",details.PricePerProduct);
                _orderDetailsHTML += string.Format("<td> {0} </td>",details.Quantity);
                _orderDetailsHTML += string.Format("<td> {0} </td>", (details.PricePerProduct * details.Quantity));
                _orderDetailsHTML += "</tr>";
                sNumber++;
            }
            _orderDetailsHTML += "<tr><td colspan='4'></td>";
            _orderDetailsHTML += string.Format("<td> Total </td>", sNumber);
            _orderDetailsHTML += string.Format("<td> {0} </td>", response.TotalAmount);
            _orderDetailsHTML += "</tr>";

            return _orderDetailsHTML;
        }

        private string GetInvoiceOrderDetailsHTML(OrderResponse response)
        {
            string _orderDetailsHTML = @"<table align='center' width='100%' border='0' cellspacing='5'><tr style='background-color:lightblue;'>
                                                    <td> S.No </td>
                                                    <td> Product </td>
                                                    <td> Product Name </td>
                                                    <td> Price </td>
                                                    <td> Quantity </td>
                                                    <td> Total Price </td>
                                                    </tr>";
            int sNumber = 1;
            foreach (var details in response.Products)
            {
                _orderDetailsHTML += "<tr>";
                _orderDetailsHTML += string.Format("<td> {0} </td>", sNumber);
                _orderDetailsHTML += string.Format("<td> <img src='{0}' width='75px' height='50px' /> </td>", details.ImageUrl);
                _orderDetailsHTML += string.Format("<td> {0} </td>", details.ProductName);
                _orderDetailsHTML += string.Format("<td> ${0} </td>", details.PricePerProduct);
                _orderDetailsHTML += string.Format("<td> {0} </td>", details.Quantity);
                _orderDetailsHTML += string.Format("<td align='right'> ${0} </td>", Math.Round(details.PricePerProduct * details.Quantity,2));
                _orderDetailsHTML += "</tr>";
                sNumber++;
            }
            _orderDetailsHTML += "<tr><td colspan='4'></td>";
            _orderDetailsHTML += string.Format("<td> Total </td>", sNumber);
            _orderDetailsHTML += string.Format("<td align='right'> ${0} </td>", response.TotalAmount);
            _orderDetailsHTML += "</tr></table>";

            return _orderDetailsHTML;
        }

        public async Task<Response<Order>> GetOrdersByCustomerId(string customerId)
        {
            var response = new Response<Order>();

            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(StoredProcedures.GET_ORDERS, sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Handle NULL or empty CustomerId
                    if (string.IsNullOrEmpty(customerId))
                    {
                        cmd.Parameters.AddWithValue("@p_CustomerId", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@p_CustomerId", customerId);
                    }

                    try
                    {
                        // Execute the stored procedure and fill the DataTable
                        DataTable tblOrders = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmd));
                        List<Order> ordersList = new List<Order>();
                        if (tblOrders.Rows.Count > 0)
                        {
                            foreach (DataRow row in tblOrders.Rows)
                            {
                                ordersList.Add(new Order
                                {
                                    OrderId = row["OrderId"].ToString() ?? "",
                                    CustomerId = row["CustomerId"].ToString(),
                                    CustomerName = row["CustomerName"].ToString() ?? "",
                                    ProductId = row["ProductId"].ToString(),
                                    ProductName = row["ProductName"].ToString() ?? "",
                                    TotalAmount = Convert.ToDouble(row["TotalAmount"]),
                                    ShippingMethodId = Convert.ToInt32(row["ShippingMethodId"]),
                                    OrderStatusId = Convert.ToInt32(row["OrderStatusId"]),
                                    TrackingNumber = row["TrackingNumber"].ToString() ?? "",
                                    OrderDetailId = row["OrderDetailId"].ToString() ?? "",
                                    Quantity = Convert.ToInt32(row["Quantity"]),
                                    PricePerProduct = Convert.ToDouble(row["PricePerProduct"]),
                                    VendorId = row["VendorId"].ToString() ?? "",
                                    ProductDescription = row["ProductDescription"].ToString() ?? "",
                                    //OrderDate = Convert.ToDateTime(row["OrderDate"])
                                    OrderDate = row["OrderDate"] != DBNull.Value ? Convert.ToDateTime(row["OrderDate"]) : DateTime.MinValue,
                                    ImageUrl = row["MainImageUrl"].ToString() ?? ""

                                });
                            }
                            response.StatusCode = 200;
                            response.Message = "Successfully Feched data.";
                            response.Result = ordersList;
                        }
                    }
                    catch (MySqlException ex)
                    {
                        response.StatusCode = 500;
                        response.Message = ex.Message;
                        response.Result = null;
                    }
                    catch (Exception ex)
                    {
                        response.StatusCode = 500;
                        response.Message = ex.Message;
                        response.Result = null;
                    }

                    return response;
                }
            }
        }

        public async Task<Response<Order>> GetCustomerOrdersByDate(BuyerOrderCriteria orderCriteria)
        {
            var response = new Response<Order>();

            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(StoredProcedures.GET_ORDERS_BY_BUYER_CRITERIA, sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_BuyerId", orderCriteria.BuyerId);
                    cmd.Parameters.AddWithValue("@p_OrderFromDate", orderCriteria.OrderFromDate);
                    cmd.Parameters.AddWithValue("@p_OrderToDate", orderCriteria.OrderToDate);

                    try
                    {
                        // Execute the stored procedure and fill the DataTable
                        DataTable tblOrders = await Task.Run(() => _isqlDataHelper.ExecuteDataTableAsync(cmd));
                        List<Order> ordersList = new List<Order>();
                        if (tblOrders.Rows.Count > 0)
                        {
                            foreach (DataRow row in tblOrders.Rows)
                            {
                                ordersList.Add(new Order
                                {
                                    OrderId = row["OrderId"].ToString() ?? "",
                                    CustomerId = row["CustomerId"].ToString(),
                                    CustomerName = row["CustomerName"].ToString() ?? "",
                                    ProductId = row["ProductId"].ToString(),
                                    ProductName = row["ProductName"].ToString() ?? "",
                                    TotalAmount = Convert.ToDouble(row["TotalAmount"]),
                                    ShippingMethodId = Convert.ToInt32(row["ShippingMethodId"]),
                                    OrderStatusId = Convert.ToInt32(row["OrderStatusId"]),
                                    TrackingNumber = row["TrackingNumber"].ToString() ?? "",
                                    OrderDetailId = row["OrderDetailId"].ToString() ?? "",
                                    Quantity = Convert.ToInt32(row["Quantity"]),
                                    PricePerProduct = Convert.ToDouble(row["PricePerProduct"]),
                                    VendorId = row["VendorId"].ToString() ?? "",
                                    ProductDescription = row["ProductDescription"].ToString() ?? "",
                                    //OrderDate = Convert.ToDateTime(row["OrderDate"])
                                    OrderDate = row["OrderDate"] != DBNull.Value ? Convert.ToDateTime(row["OrderDate"]) : DateTime.MinValue,
                                    ImageUrl = row["MainImageUrl"].ToString() ?? ""

                                });
                            }
                            response.StatusCode = 200;
                            response.Message = "Successfully Feched data.";
                            response.Result = ordersList;
                        }
                    }
                    catch (MySqlException ex)
                    {
                        response.StatusCode = 500;
                        response.Message = ex.Message;
                        response.Result = null;
                    }
                    catch (Exception ex)
                    {
                        response.StatusCode = 500;
                        response.Message = ex.Message;
                        response.Result = null;
                    }

                    return response;
                }
            }
        }       

        public async Task<Response<Order>> GetOrdersBySellerId(string VendorId)
        {
            var response = new Response<Order>();
            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(StoredProcedures.GET_ORDERS_BY_SELLER, sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Handle NULL or empty CustomerId
                    if (string.IsNullOrEmpty(VendorId))
                    {
                        cmd.Parameters.AddWithValue("p_VendorId", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("p_VendorId", VendorId);
                    }

                    try
                    {
                        // Execute the stored procedure and fill the DataTable
                        DataTable tblOrders = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmd));
                        List<Order> ordersList = new List<Order>();
                        if (tblOrders.Rows.Count > 0)
                        {
                            foreach (DataRow row in tblOrders.Rows)
                            {
                                ordersList.Add(new Order
                                {
                                    OrderId = row["OrderId"].ToString() ?? "",
                                    CustomerId = row["CustomerId"].ToString(),
                                    CustomerName = row["CustomerName"].ToString() ?? "",
                                    ProductId = row["ProductId"].ToString(),
                                    ProductName = row["ProductName"].ToString() ?? "",
                                    TotalAmount = Convert.ToDouble(row["TotalAmount"]),
                                    ShippingMethodId = Convert.ToInt32(row["ShippingMethodId"]),
                                    OrderStatusId = Convert.ToInt32(row["OrderStatusId"]),
                                    TrackingNumber = row["TrackingNumber"].ToString() ?? "",
                                    OrderDetailId = row["OrderDetailId"].ToString() ?? "",
                                    Quantity = Convert.ToInt32(row["Quantity"]),
                                    PricePerProduct = Convert.ToDouble(row["PricePerProduct"]),
                                    VendorId = row["VendorId"].ToString(),
                                    ProductDescription = row["ProductDescription"].ToString(),
                                    //OrderDate = Convert.ToDateTime(row["OrderDate"])
                                    OrderDate = row["OrderDate"] != DBNull.Value ? Convert.ToDateTime(row["OrderDate"]) : DateTime.MinValue,
                                    ImageUrl = row["MainImageUrl"].ToString() ?? "",
                                    

                                });
                            }
                            response.StatusCode = 200;
                            response.Message = "Successfully Feched data.";
                            response.Result = ordersList;
                        }
                    }
                    catch (MySqlException ex)
                    {
                        response.StatusCode = 500;
                        response.Message = ex.Message;
                        response.Result = null;
                    }
                    catch (Exception ex)
                    {
                        response.StatusCode = 500;
                        response.Message = ex.Message;
                        response.Result = null;
                    }

                    return response;
                }
            }

        }

        public async Task<Response<Order>> GetSellerOrdersByDate(SellerOrderCriteria orderCriteria)
        {
            var response = new Response<Order>();
            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(StoredProcedures.GET_ORDERS_BY_SELLER_CRITERIA, sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_VendorId", orderCriteria.SellerId);
                    cmd.Parameters.AddWithValue("@p_OrderFromDate", orderCriteria.OrderFromDate);
                    cmd.Parameters.AddWithValue("@p_OrderToDate", orderCriteria.OrderToDate);

                    try
                    {
                        // Execute the stored procedure and fill the DataTable
                        DataTable tblOrders = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmd));
                        List<Order> ordersList = new List<Order>();
                        if (tblOrders.Rows.Count > 0)
                        {
                            foreach (DataRow row in tblOrders.Rows)
                            {
                                ordersList.Add(new Order
                                {
                                    OrderId = row["OrderId"].ToString() ?? "",
                                    CustomerId = row["CustomerId"].ToString(),
                                    CustomerName = row["CustomerName"].ToString() ?? "",
                                    ProductId = row["ProductId"].ToString(),
                                    ProductName = row["ProductName"].ToString() ?? "",
                                    TotalAmount = Convert.ToDouble(row["TotalAmount"]),
                                    ShippingMethodId = Convert.ToInt32(row["ShippingMethodId"]),
                                    OrderStatusId = Convert.ToInt32(row["OrderStatusId"]),
                                    TrackingNumber = row["TrackingNumber"].ToString() ?? "",
                                    OrderDetailId = row["OrderDetailId"].ToString() ?? "",
                                    Quantity = Convert.ToInt32(row["Quantity"]),
                                    PricePerProduct = Convert.ToDouble(row["PricePerProduct"]),
                                    VendorId = row["VendorId"].ToString(),
                                    ProductDescription = row["ProductDescription"].ToString(),
                                    //OrderDate = Convert.ToDateTime(row["OrderDate"])
                                    OrderDate = row["OrderDate"] != DBNull.Value ? Convert.ToDateTime(row["OrderDate"]) : DateTime.MinValue,
                                    ImageUrl = row["MainImageUrl"].ToString() ?? "",


                                });
                            }
                            response.StatusCode = 200;
                            response.Message = "Successfully Feched data.";
                            response.Result = ordersList;
                        }
                    }
                    catch (MySqlException ex)
                    {
                        response.StatusCode = 500;
                        response.Message = ex.Message;
                        response.Result = null;
                    }
                    catch (Exception ex)
                    {
                        response.StatusCode = 500;
                        response.Message = ex.Message;
                        response.Result = null;
                    }

                    return response;
                }
            }
        }

        public async Task<Response<Order>> GetCustomersOrderedForSeller(string VendorId)
        {
            var response = new Response<Order>();
            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(StoredProcedures.GETCUSTOMERORDEREDFORSELLER, sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Handle NULL or empty CustomerId
                    if (string.IsNullOrEmpty(VendorId))
                    {
                        cmd.Parameters.AddWithValue("p_VendorId", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("p_VendorId", VendorId);
                    }

                    try
                    {
                        // Execute the stored procedure and fill the DataTable
                        DataTable tblOrders = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmd));
                        List<Order> ordersList = new List<Order>();
                        if (tblOrders.Rows.Count > 0)
                        {
                            foreach (DataRow row in tblOrders.Rows)
                            {
                                ordersList.Add(new Order
                                {
                                    OrderId = row["OrderId"].ToString() ?? "",
                                    CustomerId = row["CustomerId"].ToString(),
                                    CustomerName = row["CustomerName"].ToString() ?? "",
                                    ProductId = row["ProductId"].ToString(),
                                    ProductName = row["ProductName"].ToString() ?? "",
                                    TotalAmount = Convert.ToDouble(row["TotalAmount"]),
                                    ShippingMethodId = Convert.ToInt32(row["ShippingMethodId"]),
                                    OrderStatusId = Convert.ToInt32(row["OrderStatusId"]),
                                    TrackingNumber = row["TrackingNumber"].ToString() ?? "",
                                    OrderDetailId = row["OrderDetailId"].ToString() ?? "",
                                    Quantity = Convert.ToInt32(row["Quantity"]),
                                    PricePerProduct = Convert.ToDouble(row["PricePerProduct"]),
                                    VendorId = row["VendorId"].ToString(),
                                    ProductDescription = row["ProductDescription"].ToString(),
                                    //OrderDate = Convert.ToDateTime(row["OrderDate"])
                                    OrderDate = row["OrderDate"] != DBNull.Value ? Convert.ToDateTime(row["OrderDate"]) : DateTime.MinValue,
                                    ImageUrl = row["MainImageUrl"].ToString() ?? "",
                                    Email = row["Email"].ToString() ?? "",
                                    Mobile = row["Mobile"].ToString() ?? "",
                                    Address1 = row["Address1"].ToString() ?? ""

                                });
                            }
                            response.StatusCode = 200;
                            response.Message = "Successfully Feched data.";
                            response.Result = ordersList;
                        }
                    }
                    catch (MySqlException ex)
                    {
                        response.StatusCode = 500;
                        response.Message = ex.Message;
                        response.Result = null;
                    }
                    catch (Exception ex)
                    {
                        response.StatusCode = 500;
                        response.Message = ex.Message;
                        response.Result = null;
                    }

                    return response;
                }
            }

        }

        public async Task<Response<Order>> GetOrdersByOrderId(string orderId)
        {
            var response = new Response<Order>();
            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(StoredProcedures.GET_ORDERS_BY_ORDERID, sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Handle NULL or empty CustomerId
                    if (string.IsNullOrEmpty(orderId))
                    {
                        cmd.Parameters.AddWithValue("p_OrderId", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("p_OrderId", orderId);
                    }

                    try
                    {
                        // Execute the stored procedure and fill the DataTable
                        DataTable tblOrders = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmd));
                        List<Order> ordersList = new List<Order>();
                        if (tblOrders.Rows.Count > 0)
                        {
                            foreach (DataRow row in tblOrders.Rows)
                            {
                                ordersList.Add(new Order
                                {
                                    OrderId = row["OrderId"].ToString() ?? "",
                                    CustomerId = row["CustomerId"].ToString(),
                                    CustomerName = row["CustomerName"].ToString() ?? "",
                                    ProductId = row["ProductId"].ToString(),
                                    ProductName = row["ProductName"].ToString() ?? "",
                                    TotalAmount = Convert.ToDouble(row["TotalAmount"]),
                                    ShippingMethodId = Convert.ToInt32(row["ShippingMethodId"]),
                                    OrderStatusId = Convert.ToInt32(row["OrderStatusId"]),
                                    TrackingNumber = row["TrackingNumber"].ToString() ?? "",
                                    OrderDetailId = row["OrderDetailId"].ToString() ?? "",
                                    Quantity = Convert.ToInt32(row["Quantity"]),
                                    PricePerProduct = Convert.ToDouble(row["PricePerProduct"]),
                                    VendorId = row["VendorId"].ToString(),
                                    ProductDescription = row["ProductDescription"].ToString(),
                                    //OrderDate = Convert.ToDateTime(row["OrderDate"])
                                    OrderDate = row["OrderDate"] != DBNull.Value ? Convert.ToDateTime(row["OrderDate"]) : DateTime.MinValue,
                                    ImageUrl = row["MainImageUrl"].ToString() ?? ""

                                });
                            }
                            response.StatusCode = 200;
                            response.Message = "Successfully Feched data.";
                            response.Result = ordersList;
                        }
                    }
                    catch (MySqlException ex)
                    {
                        response.StatusCode = 500;
                        response.Message = ex.Message;
                        response.Result = null;
                    }
                    catch (Exception ex)
                    {
                        response.StatusCode = 500;
                        response.Message = ex.Message;
                        response.Result = null;
                    }

                    return response;
                }
            }

        }

        public async Task<Response<Order>> GetOrdersByDate(OrderCriteria orderCriteria)
        {
            var response = new Response<Order>();

            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(StoredProcedures.GET_ORDERS_BY_DATE, sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_OrderFromDate", orderCriteria.OrderFromDate);
                    cmd.Parameters.AddWithValue("@p_OrderToDate", orderCriteria.OrderToDate);

                    try
                    {
                        // Execute the stored procedure and fill the DataTable
                        DataTable tblOrders = await Task.Run(() => _isqlDataHelper.ExecuteDataTableAsync(cmd));
                        List<Order> ordersList = new List<Order>();
                        if (tblOrders.Rows.Count > 0)
                        {
                            foreach (DataRow row in tblOrders.Rows)
                            {
                                ordersList.Add(new Order
                                {
                                    OrderId = row["OrderId"].ToString() ?? "",
                                    CustomerId = row["CustomerId"].ToString(),
                                    CustomerName = row["CustomerName"].ToString() ?? "",
                                    ProductId = row["ProductId"].ToString(),
                                    ProductName = row["ProductName"].ToString() ?? "",
                                    TotalAmount = Convert.ToDouble(row["TotalAmount"]),
                                    ShippingMethodId = Convert.ToInt32(row["ShippingMethodId"]),
                                    OrderStatusId = Convert.ToInt32(row["OrderStatusId"]),
                                    TrackingNumber = row["TrackingNumber"].ToString() ?? "",
                                    OrderDetailId = row["OrderDetailId"].ToString() ?? "",
                                    Quantity = Convert.ToInt32(row["Quantity"]),
                                    PricePerProduct = Convert.ToDouble(row["PricePerProduct"]),
                                    VendorId = row["VendorId"].ToString() ?? "",
                                    ProductDescription = row["ProductDescription"].ToString() ?? "",
                                    //OrderDate = Convert.ToDateTime(row["OrderDate"])
                                    OrderDate = row["OrderDate"] != DBNull.Value ? Convert.ToDateTime(row["OrderDate"]) : DateTime.MinValue,
                                    ImageUrl = row["MainImageUrl"].ToString() ?? ""

                                });
                            }
                            response.StatusCode = 200;
                            response.Message = "Successfully Feched data.";
                            response.Result = ordersList;
                        }
                    }
                    catch (MySqlException ex)
                    {
                        response.StatusCode = 500;
                        response.Message = ex.Message;
                        response.Result = null;
                    }
                    catch (Exception ex)
                    {
                        response.StatusCode = 500;
                        response.Message = ex.Message;
                        response.Result = null;
                    }

                    return response;
                }
            }
        }

        public async Task<PaymentResponse> AddPayment(PaymentRequest paymentRequest)
        {
            PaymentResponse response = new PaymentResponse();
            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(StoredProcedures.ADD_PAYMENT, sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_OrderId", paymentRequest.OrderId);
                    cmd.Parameters.AddWithValue("@p_PaymentMethodId", paymentRequest.PaymentMethodId);
                    cmd.Parameters.AddWithValue("@p_PaymentStatusId", paymentRequest.PaymentStatusId);
                    cmd.Parameters.AddWithValue("@p_Amount", paymentRequest.Amount);
                    cmd.Parameters.AddWithValue("@p_PaymentId", paymentRequest.PaymentId);
                    cmd.Parameters.AddWithValue("@p_PaymentDate", paymentRequest.PaymentDate);
                    cmd.Parameters.AddWithValue("@p_InvoiceNumber", "INV-" + new Random().Next(11111111,99999999).ToString());

                    try
                    {
                        DataTable tblPayment = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmd));

                        if (tblPayment.Rows.Count > 0)
                        {
                            response.Status = 200;
                            response.PaymentId = tblPayment.Rows[0]["PaymentId"].ToString();
                            response.Message = "Success";
                        }
                        else
                        {
                            response.Status = 400;
                            response.Message = "Failed to add PayMent.";
                        }
                    }
                    catch (MySqlException ex) when (ex.Number == 500)
                    {
                        //Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertAddToCartProduct : errormessage:" + ex.Message.ToString()));
                        response.Status = 500;
                        response.Message = "ERROR : " + ex.Message;
                    }
                    catch (Exception ex)
                    {
                        //Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertAddToCartProduct : errormessage:" + ex.Message.ToString()));
                        response.Status = 500;
                        response.Message = "ERROR : " + ex.Message;
                    }
                    return response;
                }
            }

        }

        private static List<SpecialOffersResponse> MapDataTableToSpecialOffers(DataTable tbloffers)
        {
            List<SpecialOffersResponse> listspecialoffers = new List<SpecialOffersResponse>();
            foreach (DataRow offers in tbloffers.Rows)
            {
                SpecialOffersResponse item = new SpecialOffersResponse();
                item.Discount = Convert.ToInt32(offers["Discount"] != DBNull.Value ? offers["Discount"] : 0);
                item.SpecificationName = offers["SpecificationName"].ToString() ?? "";
                item.CategorySpecificationId = Convert.ToInt32(offers["CategorySpecificationId"] != DBNull.Value ? offers["CategorySpecificationId"] : 0);

                listspecialoffers.Add(item);
            }
            return listspecialoffers;
        }

        public async Task<Response<SpecialOffersResponse>> GetSpecialOffers()
        {
            Response<SpecialOffersResponse> response = new Response<SpecialOffersResponse>();
            try
            {
                MySqlCommand command = new MySqlCommand("sp_GetSpecialOffers");
                command.CommandType = CommandType.StoredProcedure;
                DataTable tbloffers = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(command));
                response.StatusCode = 200;
                response.Message = "Successfully Fetched Data.";
                response.Result = MapDataTableToSpecialOffers(tbloffers);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.Result = null;
            }
            return response;
        }

        public async Task<Response<Shipments>> AddUpdateShipmentDetail(Shipments shipmentsDetails)
        {
            Response<Shipments> response = new Response<Shipments>();
            try
            {
                MySqlCommand cmdShipments = new MySqlCommand(StoredProcedures.CUSTOMER_ADD_UPDATE_SHIPMENTSDETAILS);
                cmdShipments.CommandType = CommandType.StoredProcedure;
                cmdShipments.Parameters.AddWithValue("@p_ShipmentId", shipmentsDetails.ShipmentID);
                cmdShipments.Parameters.AddWithValue("@p_CustomerId", shipmentsDetails.CustomerId);
                cmdShipments.Parameters.AddWithValue("@p_ShipmentTypeId", shipmentsDetails.ShipmentTypeId);
                cmdShipments.Parameters.AddWithValue("@p_AccessLicenseNumber", shipmentsDetails.AccessLicenseNumber);
                cmdShipments.Parameters.AddWithValue("@p_UserID", shipmentsDetails.UserID);
                cmdShipments.Parameters.AddWithValue("@p_Password", shipmentsDetails.Password);
                cmdShipments.Parameters.AddWithValue("@p_ShipperNumber", shipmentsDetails.ShipperNumber);
                cmdShipments.Parameters.AddWithValue("@p_AccountID", shipmentsDetails.AccountID);
                cmdShipments.Parameters.AddWithValue("@p_MeterNumber", shipmentsDetails.MeterNumber);
                cmdShipments.Parameters.AddWithValue("@p_IsActive", shipmentsDetails.IsActive);
                cmdShipments.Parameters.AddWithValue("@p_CreatedDate", shipmentsDetails.CreatedDate);
                cmdShipments.Parameters.AddWithValue("@p_Key", shipmentsDetails.Key);



                DataTable tblShipments = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmdShipments));

                response.StatusCode = 200;
                response.Message = "Shipments Added/Updated Successfully.";
                response.Result = MapDataTableToShipmentsList(tblShipments);
            }
            catch (Exception ex)
            {
                //Task writeTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertProduct :  errormessage:" + ex.Message));
                // Handle the exception as needed
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Response<Shipments>> GetShipmentsByCustomerId(string customerId)
        {
            Response<Shipments> response = new Response<Shipments>();
            try
            {
                MySqlCommand cmdShipments = new MySqlCommand(StoredProcedures.CUSTOMER_GET_ALL_SHIPMENTS);
                cmdShipments.CommandType = CommandType.StoredProcedure;

                cmdShipments.Parameters.AddWithValue("@p_CustomerId", customerId);

                DataTable tblShipments = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmdShipments));

                response.StatusCode = 200;
                response.Message = "Shipments fetched Successfully.";
                response.Result = MapDataTableToShipmentsList(tblShipments);
            }
            catch (Exception ex)
            {
                //Task writeTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertProduct :  errormessage:" + ex.Message));
                // Handle the exception as needed
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return response;
        }

        #region Mapping Methods
        private OrderResponse MapDatatableToOrderResponse(DataTable tblData)
        {
            var response = new OrderResponse();
            response.OrderId = tblData.Rows[0]["OrderId"].ToString() ?? "";
            response.CustomerId = tblData.Rows[0]["CustomerId"].ToString() ?? "";
            response.CustomerEmail = tblData.Rows[0]["CustomerEmail"].ToString() ?? "";
            response.CustomerName = tblData.Rows[0]["CustomerName"].ToString() ?? "";
            response.OrderStatus = tblData.Rows[0]["OrderStatus"].ToString() ?? "";
            response.TrackingNumber = tblData.Rows[0]["TrackingNumber"].ToString() ?? "";
            response.ShippingMethod = tblData.Rows[0]["ShippingMethod"].ToString() ?? "";
            response.OrderDate = tblData.Rows[0]["OrderDate"] != DBNull.Value ? Convert.ToDateTime(tblData.Rows[0]["OrderDate"]) : DateTime.MinValue;
            response.TotalAmount = 0.0M;            
            response.Products = new List<OrderProductResponse>();
            foreach (DataRow row in tblData.Rows)
            {
                OrderProductResponse pResponse = new OrderProductResponse();
                pResponse.ProductId = row["ProductId"].ToString() ?? "";
                pResponse.SellerId = row["SellerId"].ToString() ?? "";
                pResponse.ProductName = row["ProductName"].ToString() ?? "";
                pResponse.SellerName = row["SellerName"].ToString() ?? "";
                pResponse.ImageUrl = row["ImageUrl"].ToString() ?? "";
                pResponse.Quantity = Convert.ToInt32(Convert.IsDBNull(row["Quantity"]) ? 0 : row["Quantity"]);
                pResponse.PricePerProduct = Convert.ToDecimal(Convert.IsDBNull(row["PricePerProduct"]) ? 0.0 : row["PricePerProduct"]);
                response.TotalAmount += (pResponse.PricePerProduct * pResponse.Quantity);
                response.Products.Add(pResponse);
            }
            return response;
        }

        public async Task<MemoryStream> DownloadInvoice(string orderId)
        {
            OrderResponse response = new OrderResponse();
            MemoryStream invoiceStream = new MemoryStream();
            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(StoredProcedures.GET_ORDER_INVOICE, sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_OrderId", orderId);                    

                    try
                    {
                        DataTable tblOrders = await _isqlDataHelper.ExecuteDataTableAsync(cmd);

                        if (tblOrders.Rows.Count > 0)
                        {
                            response = MapDatatableToOrderResponse(tblOrders);
                            response.Status = 200;
                            response.Message = "Success";

                            string _mailBody = EmailTemplates.CUSTOMER_INVOICE;
                            _mailBody = _mailBody.Replace("{{CUST_NAME}}", response.CustomerName);
                            _mailBody = _mailBody.Replace("{{CUST_ADDRESS1}}", response.CustomerId);
                            _mailBody = _mailBody.Replace("{{CUST_ADDRESS2}}", response.CustomerEmail);
                            _mailBody = _mailBody.Replace("{{CUST_COUNTRY}}", response.CustomerId);
                            _mailBody = _mailBody.Replace("{{CUST_PINCODE}}", response.CustomerEmail);
                            _mailBody = _mailBody.Replace("{{INVOICE_NUMBER}}", Guid.NewGuid().ToString());
                            _mailBody = _mailBody.Replace("{{INVOICE_DATE}}", response.OrderDate.ToString("MM/dd/yyyy"));
                            _mailBody = _mailBody.Replace("{{INVOICE_DUE_DATE}}", response.OrderDate.ToString("MM/dd/yyyy"));
                            //_mailBody = _mailBody.Replace("{{INVOICE_DUE_DATE}}", response.OrderDate.ToString("MM/dd/yyyy"));
                            _mailBody = _mailBody.Replace("{{PRODUCTS_DETAILS}}", GetInvoiceOrderDetailsHTML(response));
                            var invoiceDocument = new PdfDocument();
                            var sourceDoc = GetPdfFrom(_mailBody);
                            foreach (PdfPage page in sourceDoc.Pages)
                            {
                                invoiceDocument.AddPage(page);
                            }
                            invoiceDocument.Save(invoiceStream);
                            invoiceStream.Position = 0;

                            //var invoiceDocument = new PdfDocument();
                            //PdfGenerator.AddPdfPages(invoiceDocument, _mailBody, PageSize.Legal);
                            //invoiceDocument.Save(invoiceStream);
                            //invoiceStream.Position = 0; 


                            //invoiceStream = new MemoryStream(Encoding.ASCII.GetBytes(_mailBody));
                            //invoiceStream.Position = 0;

                            // await _emailHelper.SendEmail(response.CustomerEmail, "", "Invoice for your Order #" + response.OrderId, _mailBody);
                        }                       
                    }
                    catch (MySqlException ex) when (ex.Number == 500)
                    {
                        //Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertAddToCartProduct : errormessage:" + ex.Message.ToString()));
                        response.Status = 500;
                        response.Message = "ERROR : " + ex.Message;
                    }
                    catch (Exception ex)
                    {
                        //Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertAddToCartProduct : errormessage:" + ex.Message.ToString()));
                        response.Status = 500;
                        response.Message = "ERROR : " + ex.Message;
                    }
                    return invoiceStream;
                }
            }
        }

        public async Task<string> DownloadInvoiceHtml(string orderId)
        {
            OrderResponse response = new OrderResponse();
            string invoiceHtml = "";
            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(StoredProcedures.GET_ORDER_INVOICE, sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_OrderId", orderId);

                    try
                    {
                        DataTable tblOrders = await _isqlDataHelper.ExecuteDataTableAsync(cmd);

                        if (tblOrders.Rows.Count > 0)
                        {
                            response = MapDatatableToOrderResponse(tblOrders);
                            response.Status = 200;
                            response.Message = "Success";

                            string _mailBody = EmailTemplates.CUSTOMER_INVOICE;
                            _mailBody = _mailBody.Replace("{{CUST_NAME}}", response.CustomerName);
                            _mailBody = _mailBody.Replace("{{CUST_ADDRESS1}}", response.CustomerId);
                            _mailBody = _mailBody.Replace("{{CUST_ADDRESS2}}", response.CustomerEmail);
                            _mailBody = _mailBody.Replace("{{CUST_COUNTRY}}", response.CustomerId);
                            _mailBody = _mailBody.Replace("{{CUST_PINCODE}}", response.CustomerEmail);
                            _mailBody = _mailBody.Replace("{{INVOICE_NUMBER}}", Guid.NewGuid().ToString());
                            _mailBody = _mailBody.Replace("{{INVOICE_DATE}}", response.OrderDate.ToString("MM/dd/yyyy"));                            
                            _mailBody = _mailBody.Replace("{{INVOICE_DUE_DATE}}", response.OrderDate.ToString("MM/dd/yyyy"));
                            _mailBody = _mailBody.Replace("{{PRODUCTS_DETAILS}}", GetInvoiceOrderDetailsHTML(response));
                            invoiceHtml = _mailBody;
                        }
                    }
                    catch (MySqlException ex) when (ex.Number == 500)
                    {
                        //Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertAddToCartProduct : errormessage:" + ex.Message.ToString()));
                        response.Status = 500;
                        response.Message = "ERROR : " + ex.Message;
                    }
                    catch (Exception ex)
                    {
                        //Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertAddToCartProduct : errormessage:" + ex.Message.ToString()));
                        response.Status = 500;
                        response.Message = "ERROR : " + ex.Message;
                    }
                    return invoiceHtml;
                }
            }
        }

        private PdfDocument GetPdfFrom(string htmlString)
        {
            var pdfDoc = new HtmlToPdf();
            pdfDoc.Options.PdfPageSize = PdfPageSize.Legal;
            pdfDoc.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            pdfDoc.Options.WebPageFixedSize = true;
            pdfDoc.Options.WebPageWidth = 1024;
            pdfDoc.Options.WebPageHeight = 768;
            pdfDoc.Options.MarginLeft = 0;
            pdfDoc.Options.MarginRight = 0;
            pdfDoc.Options.MarginTop = 0;
            pdfDoc.Options.MarginBottom = 0;
            pdfDoc.Options.RenderingEngine = RenderingEngine.WebKitRestricted;
            var doc = pdfDoc.ConvertHtmlString(htmlString);
            return doc;
        }

        public async Task<OrderResponse> SendInvoiceByMail(string orderId)
        {
            OrderResponse response = new OrderResponse();
            MemoryStream invoiceStream = new MemoryStream();
            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(StoredProcedures.GET_ORDER_INVOICE, sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_OrderId", orderId);

                    try
                    {
                        DataTable tblOrders = await _isqlDataHelper.ExecuteDataTableAsync(cmd);

                        if (tblOrders.Rows.Count > 0)
                        {
                            response = MapDatatableToOrderResponse(tblOrders);
                            response.Status = 200;
                            response.Message = "Success";

                            string _mailBody = EmailTemplates.CUSTOMER_INVOICE;
                            _mailBody = _mailBody.Replace("{{CUST_NAME}}", response.CustomerName);
                            _mailBody = _mailBody.Replace("{{CUST_ADDRESS1}}", response.CustomerId);
                            _mailBody = _mailBody.Replace("{{CUST_ADDRESS2}}", response.CustomerEmail);
                            _mailBody = _mailBody.Replace("{{CUST_COUNTRY}}", response.CustomerId);
                            _mailBody = _mailBody.Replace("{{CUST_PINCODE}}", response.CustomerEmail);
                            _mailBody = _mailBody.Replace("{{INVOICE_NUMBER}}", Guid.NewGuid().ToString());
                            _mailBody = _mailBody.Replace("{{INVOICE_DATE}}", response.OrderDate.ToString("MM/dd/yyyy"));
                            _mailBody = _mailBody.Replace("{{INVOICE_DUE_DATE}}", response.OrderDate.ToString("MM/dd/yyyy"));
                            _mailBody = _mailBody.Replace("{{PRODUCTS_DETAILS}}", GetInvoiceOrderDetailsHTML(response));
                            //var invoiceDocument = new PdfDocument();
                            //var sourceDoc = GetPdfFrom(_mailBody);
                            //foreach (PdfPage page in sourceDoc.Pages)
                            //{
                            //    invoiceDocument.AddPage(page);
                            //}
                            //invoiceDocument.Save(invoiceStream);
                            //invoiceStream.Position = 0;

                            //var invoiceDocument = new PdfDocument();
                            //PdfGenerator.AddPdfPages(invoiceDocument, _mailBody, PageSize.A4);
                            //invoiceDocument.Save(invoiceStream);
                            //invoiceStream.Position = 0;

                            await _emailHelper.SendEmail(response.CustomerEmail, "", "Invoice for your Order #" + response.OrderId, _mailBody, invoiceStream);                           
                        }
                    }
                    catch (MySqlException ex) when (ex.Number == 500)
                    {
                        //Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertAddToCartProduct : errormessage:" + ex.Message.ToString()));
                        response.Status = 500;
                        response.Message = "ERROR : " + ex.Message;
                    }
                    catch (Exception ex)
                    {
                        //Task WriteTask = Task.Factory.StartNew(() => LogFileException.Write_Log_Exception(_exPathToSave, "InsertAddToCartProduct : errormessage:" + ex.Message.ToString()));
                        response.Status = 500;
                        response.Message = "ERROR : " + ex.Message;
                    }
                    return response;
                }
            }
        }

        private List<Shipments> MapDataTableToShipmentsList(DataTable tblShipments)
        {
            List<Shipments> lstShipments = new List<Shipments>();
            foreach (DataRow aItem in tblShipments.Rows)
            {
                Shipments shipment = new Shipments();
                shipment.ShipmentID = aItem["ShipmentID"].ToString() ?? "";
                shipment.ShipmentTypeId = Convert.ToInt32(Convert.IsDBNull(aItem["ShipmentTypeId"]) ? 0 : aItem["ShipmentTypeId"]);
                shipment.CustomerId = aItem["CustomerId"].ToString() ?? "";
                shipment.AccessLicenseNumber = aItem["AccessLicenseNumber"].ToString() ?? "";
                shipment.UserID = aItem["UserID"].ToString() ?? "";
                shipment.Password = aItem["Password"].ToString() ?? "";
                shipment.ShipperNumber = aItem["ShipperNumber"].ToString() ?? "";
                shipment.AccountID = aItem["AccountID"].ToString() ?? "";
                shipment.MeterNumber = aItem["MeterNumber"].ToString() ?? "";
                shipment.IsActive = Convert.ToDecimal(Convert.IsDBNull(aItem["IsActive"]) ? 0 : aItem["IsActive"]) == 1;
                shipment.CreatedDate = Convert.ToDateTime(Convert.IsDBNull(aItem["CreatedDate"]) ? (DateTime?)null : aItem["CreatedDate"]);
                shipment.Key = aItem["Key"].ToString() ?? "";
                lstShipments.Add(shipment);
            }
            return lstShipments;
        }
        #endregion Mapping Methods
    }
}
