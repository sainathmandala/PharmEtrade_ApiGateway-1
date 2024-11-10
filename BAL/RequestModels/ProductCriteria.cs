using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.RequestModels
{
    public class ProductCriteria
    {
        public string? CustomerId { get; set; }
        public string? Deals { get; set; }
        public string? Brands { get; set; }
        public string? Generics { get; set;}
        public int Discount { get; set; }
        public int Expiring { get; set; }
        public string? WholeSeller { get; set; }
        public bool PharmacyItems { get; set; }
        public bool PrescriptionDrugs { get; set; }
        public bool OTCProducts { get; set; }
        public string? VAWDSeller { get; set; }
        public bool TopSellingProducts { get; set; }
        public bool BuyAgain { get; set; }
        public int ProductCategoryId { get; set; }
        public int CategorySpecificationId { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? NDCUPC { get; set; }
        public DateTime? SalePriceValidFrom { get; set; }
        public DateTime? SalePriceValidTo { get; set; }
        public string? ProductName { get; set; }
    }
}
