using System.ComponentModel.DataAnnotations;

namespace BAL.ViewModels
{
    public class ProductFilter
    {
        [Required]
        public int AddproductID { get; set; }
        [Required]
        public int Productcategory_id { get; set; }
        [Required]
        public int ImageID { get; set; }
        [Required]
        public int Sizeid { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Product Name cannot exceed 100 characters.")]
        public string ProductName { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "NDC or UPC cannot exceed 50 characters.")]
        public string NDCorUPC { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Brand Name cannot exceed 50 characters.")]
        public string BrandName { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal PriceName { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "UPN member price must be a positive value.")]
        public decimal UPNmemberPrice { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Amount in stock must be a positive value.")]
        public int AmountInStock { get; set; }
        [Required]
        public bool Taxable { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Sale price must be a positive value.")]
        public decimal SalePrice { get; set; }
        [DataType(DataType.Date)]
        public DateTime SalePriceFrom { get; set; }
        [DataType(DataType.Date)]
        public DateTime SalePriceTo { get; set; }
        [StringLength(100, ErrorMessage = "Manufacturer cannot exceed 100 characters.")]
        public string Manufacturer { get; set; }
        [StringLength(50, ErrorMessage = "Strength cannot exceed 50 characters.")]
        public string Strength { get; set; }
        [DataType(DataType.Date)]
        public DateTime Fromdate { get; set; }
        [StringLength(50, ErrorMessage = "Lot number cannot exceed 50 characters.")]
        public string LotNumber { get; set; }
        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Pack quantity must be a positive value.")]
        public int PackQuantity { get; set; }
        [StringLength(50, ErrorMessage = "Pack type cannot exceed 50 characters.")]
        public string PackType { get; set; }
        [StringLength(100, ErrorMessage = "Pack condition cannot exceed 100 characters.")]
        public string PackCondition { get; set; }
        [StringLength(500, ErrorMessage = "Product description cannot exceed 500 characters.")]
        public string ProductDescription { get; set; }
    }
}
