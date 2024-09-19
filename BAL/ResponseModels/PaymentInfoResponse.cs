using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ResponseModels
{
    public class PaymentInfoResponse
    {

        public string PaymentInfoId { get; set; }
        public string OrderId { get; set; }
        public int PaymentMethodId { get; set; }
        public string CardNumber { get; set; }
        public string CardType { get; set; }
        public string CVV { get; set; }
        public string ValidThrough { get; set; }
        public string NameOnCard { get; set; }
        public string Bank { get; set; }
        public decimal PaymentAmount { get; set; }
        public bool IsCreditCard { get; set; }
        public int StatusId { get; set; }
        public DateTime PaymentDate { get; set; }

    }
}
