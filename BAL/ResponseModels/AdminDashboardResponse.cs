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
    }

    public class AdminDashboardResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        //public List<CustomersCount> Customers { get; set; }
        //public List<ProductsPerCategory> Products { get; set; }
        //public AdminDashboardResponse() {
        //    this.Products = new List<ProductsPerCategory>();
        //    this.Customers = new List<CustomersCount>();
        //}
        public int TotalCustomers { get; set; }
        public Dictionary<string,string> CountsPerTypes { get; set; }
        public int TotalOrders { get; set; }
        public int TotalProducts { get; set; }
        public int TotalActiveProducts { get; set; }
    }
}
