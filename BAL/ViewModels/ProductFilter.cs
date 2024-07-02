namespace BAL.ViewModels
{
    public class ProductFilter
    {
        public int AddproductID { get; set; }
        public int Productcategory_id { get; set; }
        public int ImageID { get; set; }
        public int Sizeid { get; set; }
        public string ProductName { get; set; }
        public string NDCorUPC { get; set; }
        public string BrandName { get; set; }
        public string PriceName { get; set; }
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
    }
}
