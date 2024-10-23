using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class CustomerAddress
    {
       public string? AddressId {get;set;}
        public string CustomerId { get;set;}
        public string? FirstName { get;set;}
        public string? MiddleName { get;set;}
        public string? LastName { get;set;}
        public string? PhoneNumber { get;set;}
        public string? Pincode { get;set;}
        public string? Address1 { get;set;}
        public string? Address2 { get;set;}
        public string? Landmark { get;set;}
        public string? City { get;set;}
        public string? State { get;set;}
        public string? Country { get;set;}
        public bool IsDefault { get;set;}
        public int AddressTypeId { get;set;}
        public string? DeliveryInstructions { get;set;}
    }
}
