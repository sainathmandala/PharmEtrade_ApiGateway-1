using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class Product
    {
        public string ProductID{get; set;}
        public int ProductCategoryId { get; set;}
        public int ProductGalleryId { get; set;}
        public int ProductSizeId { get; set;}
        public string ProductName { get; set;}
        public string NDCorUPC { get; set;}
        public string BrandName { get; set;}
        public string PriceName { get; set;}
        public decimal UPNMemberPrice { get; set;}
        public int AmountInStock { get; set;}
        public bool Taxable { get; set;}
        public decimal SalePrice { get; set;}
        public DateTime SalePriceValidFrom { get; set;}
        public DateTime SalePriceValidTo { get; set;}
        public string Manufacturer { get; set;}
        public string Strength { get; set;}
        public DateTime AvailableFromDate { get; set;}
        public string LotNumber { get; set;}
        public DateTime ExpiryDate { get; set;}
        public int PackQuantity { get; set;}
        public string PackType { get; set;}
        public string PackCondition { get; set;}
        public string ProductDescription { get; set;}
        public string MetaKeywords { get; set;}
        public string MetaTitle { get; set;}
        public string MetaDescription { get; set;}
        public string SaltComposition { get; set;}
        public string UriKey { get; set;}
        public string AboutTheProduct { get; set;}
        public int CategorySpecificationId { get; set;}
        public int ProductTypeId { get; set;}
        public string SellerId { get; set;}
        public string ImageUrl { get; set; }
        public string? Caption { get; set; }
    }
}
