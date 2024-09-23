using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class AddProduct
    {
        public int AddproductId { get; set; }
        public int? ProductcategoryId { get; set; }
        public int? ImageId { get; set; }
        public int? Sizeid { get; set; }
        public string? ProductName { get; set; }
        public string? NdcorUpc { get; set; }
        public string? BrandName { get; set; }
        public decimal? PriceName { get; set; }
        public decimal? UpnmemberPrice { get; set; }
        public int? AmountInStock { get; set; }
        public bool? Taxable { get; set; }
        public decimal? SalePrice { get; set; }
        public DateTime? SalePriceFrom { get; set; }
        public DateTime? SalePriceTo { get; set; }
        public string? Manufacturer { get; set; }
        public string? Strength { get; set; }
        public DateTime? Fromdate { get; set; }
        public string? LotNumber { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int? PackQuantity { get; set; }
        public string? PackType { get; set; }
        public string? PackCondition { get; set; }
        public string? ProductDescription { get; set; }

        //public virtual ProductGallery? Image { get; set; }
        public virtual Category? Productcategory { get; set; }
        //public virtual ProductSize? Size { get; set; }
    }
}
