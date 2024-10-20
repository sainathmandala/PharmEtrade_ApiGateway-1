using BAL.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ResponseModels
{
    public class CustomersCount
    {
        public int CustomerTypeId { get; set; }
        public string? Description { get; set; }
        public int Count { get; set; }
        public int ActiveCount { get; set; }
        public int InActiveCount { get; set; }
    }

    public class OrdersCount
    {
        public int OrderStatusId { get; set; }
        public int Count { get; set; }
        public int ActiveCount { get; set; }
        public int InActiveCount { get; set; }
    }

    public class AdminDashboardResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalActiveCustomers { get; set; }
        public int TotalInActiveCustomers { get; set; }
        public List<CustomersCount> CustomersCounts { get; set; } = new List<CustomersCount>();
        public int TotalOrders { get; set; }
        public List<OrdersCount> OrdersCounts { get; set; } = new List<OrdersCount>();
        public int TotalProducts { get; set; }
        public int TotalActiveProducts { get; set; }
        public int TotalInActiveProducts { get; set; }
    }
}
