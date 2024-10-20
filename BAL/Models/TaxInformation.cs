using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class TaxInformation
    {
        public string TaxInformationID { get; set; }
        public string StateName { get; set; }
        public int CategorySpecificationID { get; set; }
        public int TaxPercentage { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int IsActive { get; set; }
    }
}
