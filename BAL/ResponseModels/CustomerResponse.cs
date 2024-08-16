using BAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ResponseModels
{
    public  class CustomerResponse
    {
            
        public Customer CustomerDetails { get; set; }
        public CustomerBusinessInfo BusinessInfo { get; set; }
    }
}

