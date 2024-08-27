using BAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ResponseModels
{
    public class ProductResponse
    {
        public ProductResponse() { 
            this.CategorySpecification = new CategorySpecification();
            this.ProductCategory = new ProductCategory();
            this.ProductGallery = new ProductGallery();
            this.ProductType = new ProductType();
            this.ProductSize = new ProductSize();
        }
        public string ProductID { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public ProductGallery ProductGallery { get; set; }
        public ProductSize ProductSize { get; set; }
        public string ProductName { get; set; }
        public string NDCorUPC { get; set; }
        public string BrandName { get; set; }
        public string PriceName { get; set; }
        public decimal UPNMemberPrice { get; set; }
        public int AmountInStock { get; set; }
        public bool Taxable { get; set; }
        public decimal SalePrice { get; set; }
        public DateTime SalePriceValidFrom { get; set; }
        public DateTime SalePriceValidTo { get; set; }
        public string Manufacturer { get; set; }
        public string Strength { get; set; }
        public DateTime AvailableFromDate { get; set; }
        public string LotNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int PackQuantity { get; set; }
        public string PackType { get; set; }
        public string PackCondition { get; set; }
        public string ProductDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string SaltComposition { get; set; }
        public string UriKey { get; set; }
        public string AboutTheProduct { get; set; }
        public CategorySpecification CategorySpecification { get; set; }
        public ProductType ProductType { get; set; }
        public string SellerId { get; set; }
        public string? States { get; set; }
    }
}
