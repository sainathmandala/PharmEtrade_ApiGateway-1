using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ViewModels
{
    public class ProductViewModel
    {
        public int statusCode { get; set; }
        public string message { get; set; }
        public List<ProductFilter> Productfilter { get; set; }
    }
}
