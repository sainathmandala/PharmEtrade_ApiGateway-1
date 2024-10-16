using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class BeneficiaryDetails
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string? BankName { get; set; }
        public string? BankAddress { get; set; }
        public string? RoutingNumber { get; set; }
        public string? AccountNumber { get; set; }
        public string? AccountType { get; set; }
        public string? CheckPayableTo { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public int Zip { get; set; }
        
    }
}
