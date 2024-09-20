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



namespace BAL.BusinessLogic.Helper
{
    public class OrdersHelper : IOrders
    {
        private readonly IsqlDataHelper _isqlDataHelper;
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public OrdersHelper(IConfiguration configuration, IsqlDataHelper isqlDataHelper)
        {
            _configuration = configuration;
            _isqlDataHelper = isqlDataHelper;
            _connectionString = configuration.GetConnectionString("APIDBConnectionString");
        }
        public async Task<OrderResponse> AddOrder(OrderRequest orderRequest)
        {
            OrderResponse response = new OrderResponse();
            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(StoredProcedures.ADD_UPDATE_ORDER, sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

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
                            response.CustomerName = tblOrders.Rows[0]["CustomerName"].ToString();
                            response.ProductName = tblOrders.Rows[0]["ProductName"].ToString();
                            response.OrderId = tblOrders.Rows[0]["OrderId"].ToString();

                            //response.VendorName = tblOrders.Rows[0][""].ToString();
                            response.Message = "Success";
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
                                    CustomerName = row["CustomerName"].ToString()??"",
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
                                    OrderId = row["OrderId"].ToString(),
                                    CustomerId = row["CustomerId"].ToString(),
                                    CustomerName = row["CustomerName"].ToString(),
                                    ProductId = row["ProductId"].ToString(),
                                    ProductName = row["ProductName"].ToString(),
                                    TotalAmount = Convert.ToDouble(row["TotalAmount"]),
                                    ShippingMethodId = Convert.ToInt32(row["ShippingMethodId"]),
                                    OrderStatusId = Convert.ToInt32(row["OrderStatusId"]),
                                    TrackingNumber = row["TrackingNumber"].ToString(),
                                    OrderDetailId = row["OrderDetailId"].ToString(),
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
                item.Discount = Convert.ToInt32(offers["Discount"]!=DBNull.Value ? offers["Discount"]:0);
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
            return response ;
        }

    }
}
