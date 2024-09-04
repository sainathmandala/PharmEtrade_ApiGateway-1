using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.RequestModels
{
    public class ProductCriteria
    {
        public string? Deals { get; set; }
        public string? Brands { get; set; }
        public string? Generics { get; set;}
        public int Discount { get; set; }
        public int Expiring { get; set; }
        public string? WholeSeller { get; set; }
        public string? PharmacyItems { get; set; }
        public string? PrescriptionDrugs { get; set; }
        public string? OTCProducts { get; set; }
        public string? VAWDSeller { get; set; }
        public string? TopSellingProducts { get; set; }
        public string? BuyAgain { get; set; }

    }
}
