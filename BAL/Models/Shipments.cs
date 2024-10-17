using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class Shipments
    {
        public string ShipmentID { get; set; }
        public int ShipmentTypeId { get; set; }
        public string CustomerId { get; set; }
        public string AccessLicenseNumber { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
        public string ShipperNumber { get; set; }
        public string AccountID { get; set; }
        public string MeterNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
