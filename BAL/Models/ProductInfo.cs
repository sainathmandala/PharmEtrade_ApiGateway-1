using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class ProductInfo
    {
        public string ProductID { get; set; }
        public int ProductCategoryId { get; set; }
        public string ProductName { get; set; }
        public string NDCorUPC { get; set; }
        public string BrandName { get; set; }
        public string Size { get; set; }        
        public string Manufacturer { get; set; }
        public string Strength { get; set; }        
        public string LotNumber { get; set; }
        public DateTime AvailableFromDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int PackQuantity { get; set; }
        public string PackType { get; set; }
        public string PackCondition { get; set; }
        public string ProductDescription { get; set; }        
        public string AboutTheProduct { get; set; }
        public int CategorySpecificationId { get; set; }
        public int ProductTypeId { get; set; }
        public string SellerId { get; set; }        
        public string? States { get; set; }
        public string UnitOfMeasure { get; set; }
    }
}
