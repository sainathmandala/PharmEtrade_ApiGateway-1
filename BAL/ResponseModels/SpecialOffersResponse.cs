using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ResponseModels
{
    public  class SpecialOffersResponse
    {

        public int Discount { get; set; }
        public string SpecificationName { get; set; }
        public int CategorySpecificationId { get; set; }
    }
}
