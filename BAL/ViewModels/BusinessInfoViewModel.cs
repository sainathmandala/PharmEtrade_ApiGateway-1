using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ViewModels
{
    public class BusinessInfoViewModel
    {
        public int BusinessInfoId { get; set; }
        public int UserId { get; set; }
        public string? ShopName { get; set; }
        public string? DBA { get; set; }
        public string? LegalBusinessName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? BusinessPhone { get; set; }
        public string? BusinessFax { get; set; }
        public string? BusinessEmail { get; set; }
        public string FederalTaxId { get; set; }
        public string? DEA { get; set; }
        public string? PharmacyLicence { get; set; }
        public DateTime? DEAExpirationDate { get; set; }
        public DateTime? PharmacyLicenseExpirationDate { get; set; }
        public IFormFile? DEAlicenseCopy { get; set; }
        public IFormFile? PharmacyLicenseCopy { get; set; }
        public string? NPI { get; set; }
        public string? NCPDP { get; set; }
    }
}
