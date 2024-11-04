using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ResponseModels
{
    public class PaymentResponse
    {
        public string? PaymentId { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
    }
}
