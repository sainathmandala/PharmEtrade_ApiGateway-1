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
using BAL.ViewModels;

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
            _connectionString = configuration.GetConnectionString("OnlineexamDB");            
        }
        public async Task<OrderResponse> AddOrder(OrderRequest orderRequest)
        {
            OrderResponse response = new OrderResponse();
            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_InsertOrder", sqlcon))
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

        //public async Task<List<Order>> GetOrdersByCustomerId(string customerId)
        //{
        //    List<Order> ordersList = new List<Order>();

        //    using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
        //    {
        //        using (MySqlCommand cmd = new MySqlCommand("sp_GetOrders", sqlcon))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@p_CustomerId", customerId);

        //            try
        //            {
        //                DataTable tblOrders = await Task.Run(() => _isqlDataHelper.SqlDataAdapterasync(cmd));

        //                if (tblOrders.Rows.Count > 0)
        //                {
        //                    foreach (DataRow row in tblOrders.Rows)
        //                    {
        //                        ordersList.Add(new Order
        //                        {
        //                            OrderId = row["OrderId"].ToString(),
        //                            CustomerId = row["CustomerId"].ToString(),
        //                            CustomerName = row["CustomerName"].ToString(),
        //                            ProductId = Convert.ToInt32(row["ProductId"]),
        //                            ProductName = row["ProductName"].ToString(),
        //                            TotalAmount = Convert.ToDouble(row["TotalAmount"]),
        //                            ShippingMethodId = Convert.ToInt32(row["ShippingMethodId"]),
        //                            OrderStatusId = Convert.ToInt32(row["OrderStatusId"]),
        //                            TrackingNumber = row["TrackingNumber"].ToString(),
        //                            OrderDetailId = row["OrderDetailId"].ToString(),
        //                            Quantity = Convert.ToInt32(row["Quantity"]),
        //                            PricePerProduct = Convert.ToDouble(row["PricePerProduct"]),
        //                            VendorId = row["VendorId"].ToString(),
        //                            ProductDescription = row["ProductDescription"].ToString(),
        //                            OrderDate = Convert.ToDateTime(row["OrderDate"])
        //                        });
        //                    }
        //                }

        //            }

        //            catch (MySqlException ex)
        //            {
        //                ;
        //            }
        //            catch (Exception ex)
        //            {

        //            }


        //            return ordersList;
        //        }
        //    }
        //}


        public async Task<List<Order>> GetOrdersByCustomerId(string customerId)
        {
            List<Order> ordersList = new List<Order>();

            using (MySqlConnection sqlcon = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_GetOrders", sqlcon))
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

                        if (tblOrders.Rows.Count > 0)
                        {
                            foreach (DataRow row in tblOrders.Rows)
                            {
                                ordersList.Add(new Order
                                {
                                    OrderId = row["OrderId"].ToString(),
                                    CustomerId = row["CustomerId"].ToString(),
                                    CustomerName = row["CustomerName"].ToString(),
                                    ProductId = Convert.ToInt32(row["ProductId"]),
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
                                    OrderDate = row["OrderDate"] != DBNull.Value ? Convert.ToDateTime(row["OrderDate"]) : DateTime.MinValue
                                });
                            }
                        }
                    }
                    catch (MySqlException ex)
                    {
                        // Handle MySQL exceptions
                        // Log the exception or throw
                    }
                    catch (Exception ex)
                    {
                        // Handle general exceptions
                        // Log the exception or throw
                    }

                    return ordersList;
                }
            }
        }



    }
}
