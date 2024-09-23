using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class ProductPrice
    {
        public string ProductPriceId { get; set; }
        public string ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UPNMemberPrice { get; set; }
        public int Discount { get; set; }
        public decimal SalePrice { get; set; }
        public DateTime? SalePriceValidFrom { get; set; }
        public DateTime? SalePriceValidTo { get; set; }

        public bool Taxable { get; set; }
        public bool ShippingCostApplicable { get; set; }
        public decimal ShippingCost { get; set; }
        public int AmountInStock { get; set; }
    }
}
