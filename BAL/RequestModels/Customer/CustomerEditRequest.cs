﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.RequestModels.Customer
{
    public class CustomerEditRequest
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
    }
}
