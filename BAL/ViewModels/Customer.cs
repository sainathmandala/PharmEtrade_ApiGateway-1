using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ViewModels
{
    public class Customer
    {
        public string? CustomerId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? Password { get; set; }
        public int CustomerTypeId { get; set; }
        public int AccountTypeId { get; set; }
        public int IsUPNMember { get; set; }
        public string? LoginOTP { get; set; }
        public DateTime? OTPExpiryDate { get; set; }

        public DateTime? CreatedDate { get;set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? ActivationDate { get; set; }
        public string? ShopName { get; set; }
        public int IsActive { get; set; }
    }
}
