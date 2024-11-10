using DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class ProductBasicDetails
    {
        public string? ProductID {get;set;}
        public int? ProductCategoryId { get;set;}
        public string? ProductGalleryId { get; set;}
        public string? ProductName { get; set;}
        public string? BrandName { get; set;}
        public string? Manufacturer { get; set;}
        public string? ImageUrl { get; set;}
        public string? Caption { get; set;}
        public string? UriKey { get; set; }
        public decimal SalePrice { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int MaximumOrderQuantity { get; set; }
        public int MinimumOrderQuantity { get; set; }
        public int AmountInStock { get; set; }
        public string? SellerId { get; set; }
        public string? SellerName { get; set; }
        public bool? IsShippingCostApplicable { get; set; }
        public decimal? ShippingCost { get; set; }
        public DateTime? SalePriceValidFrom { get; set; }
        public DateTime? SalePriceValidTo { get; set; }
    }
}
