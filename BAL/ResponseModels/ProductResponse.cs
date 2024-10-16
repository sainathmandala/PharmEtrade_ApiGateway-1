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
        public ProductResponse()
        {
            this.CategorySpecification = new CategorySpecification();
            this.ProductCategory = new ProductCategory();
            this.ProductGallery = new ProductGallery();
            //this.Price = new ProductPrice();
            //this.ProductInfo = new ProductInfo();            
        }
        //public ProductInfo ProductInfo { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public ProductGallery ProductGallery { get; set; }
        //public ProductPrice Price { get; set; }        
        public CategorySpecification CategorySpecification { get; set; }        

        // Product Info
        public string ProductID { get; set; }
        public int ProductCategoryId { get; set; }
        public string ProductName { get; set; }
        public string NDCorUPC { get; set; }
        public string BrandName { get; set; }
        public string Size { get; set; }
        public string Manufacturer { get; set; }
        public string Strength { get; set; }
        public string LotNumber { get; set; }
        public DateTime? AvailableFromDate { get; set; }
        public string FormattedAvailableFromDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool IsFullPack { get; set; }
        public string FormattedExpiryDate { get; set; }
        public int PackQuantity { get; set; }
        public string PackType { get; set; }
        public string PackCondition { get; set; }
        public string ProductDescription { get; set; }
        public string AboutTheProduct { get; set; }
        public int CategorySpecificationId { get; set; }
        public int ProductTypeId { get; set; }
        public string SKU { get; set; }
        public string SellerId { get; set; }
        public string SellerFirstName { get; set; }
        public string SellerLastName { get; set; }
        public string? States { get; set; }
        public string UnitOfMeasure { get; set; }
        public string? Form { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Length { get; set; }
        public decimal Weight { get; set; }
        public string? MainImageUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }

        // Price
        public string ProductPriceId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UPNMemberPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal SalePrice { get; set; }
        public DateTime? SalePriceValidFrom { get; set; }
        public DateTime? SalePriceValidTo { get; set; }
        public bool Taxable { get; set; }
        public bool ShippingCostApplicable { get; set; }
        public decimal ShippingCost { get; set; }
        public int AmountInStock { get; set; }

    }
}
