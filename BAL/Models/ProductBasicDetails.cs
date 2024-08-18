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
        public int? ProductGalleryId { get; set;}
        public string? ProductName { get; set;}
        public decimal SalePrice { get; set;}
        public string? BrandName { get; set;}
        public string? Manufacturer { get; set;}
        public string? UriKey { get; set;}
        public string? ImageUrl { get; set;}
        public string? Caption { get; set;}
    }
}
