using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BAL.ViewModels
{
    public class ProductFilter
    {
       
        public int AddproductID { get; set; }  
        public int Productcategory_id { get; set; }       
        public int Sizeid { get; set; }
        public string ProductName { get; set; }    
      
        public string NDCorUPC { get; set; }
        
        public string BrandName { get; set; }
       
        public decimal PriceName { get; set; }
        
        public decimal UPNmemberPrice { get; set; }
       
        public int AmountInStock { get; set; }
       
        public bool Taxable { get; set; }
       
        public decimal SalePrice { get; set; }
      
        public DateTime SalePriceFrom { get; set; }
       
        public DateTime SalePriceTo { get; set; }
       
        public string Manufacturer { get; set; }
       
        public string Strength { get; set; }
        
        public DateTime Fromdate { get; set; }
      
        public string LotNumber { get; set; }
       
        public DateTime ExpirationDate { get; set; }
       
        public int PackQuantity { get; set; }
       
        public string PackType { get; set; }
      
        public string PackCondition { get; set; }
       
        public string ProductDescription { get; set; }

        public IFormFile ImageUrl { get; set; }
        public string Caption { get; set; }
        
        public string MetaKeywords { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string SaltComposition { get; set; }
        public string UriKey { get; set; }
        public string AboutTheProduct { get; set; }
        public int CategorySpecificationId { get; set; }
        public int ProductTypeId { get; set; }
        public string SellerId { get; set; }
    }
}
