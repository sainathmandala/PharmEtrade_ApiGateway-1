using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.RequestModels
{
    public class CustomerBusinessInfo1
    {
        public int CustomerBusinessInfoId { get; set; } // Auto-increment primary key
        public string CustomerId { get; set; } // Foreign key to Customers table
        public string ShopName { get; set; }
        public string DBA { get; set; }
        public string LegalBusinessName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string BusinessPhone { get; set; }
        public string BusinessFax { get; set; }
        public string BusinessEmail { get; set; }
        public string FederalTaxId { get; set; }
        public string DEA { get; set; }
        public string PharmacyLicence { get; set; }
        public DateTime DEAExpirationDate { get; set; }
        public DateTime PharmacyLicenseExpirationDate { get; set; }
        public string DEALicenseCopy { get; set; }
        public string PharmacyLicenseCopy { get; set; }
        public string NPI { get; set; }
        public string NCPDP { get; set; }
    }
}
