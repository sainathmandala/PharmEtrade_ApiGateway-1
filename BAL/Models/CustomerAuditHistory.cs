using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class CustomerAuditHistory
    {
        public string CustomerAuditHistoryId { get; set; }  
        public string CustomerId { get; set; }
        public string Comments { get; set; }    
        public string Action { get; set; }  
        public DateTime AuditDate { get; set; }
    }
}
