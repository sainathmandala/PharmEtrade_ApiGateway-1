using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ResponseModels
{
    public class BusinessInfoResponse
    {
        public int Status { get; set; }
        public string CustomerId { get; set; }
        public int CustomerBusinessInfoId { get; set; }
        public string Message { get; set; }
    }
}
