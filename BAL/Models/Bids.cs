using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class Bids
    {
        public string BidId {get;set;}
        public string BuyerId { get;set;}
        public string ProductId { get;set;}
        public decimal Price { get;set;}
        public int Quantity { get;set;}
        public string Comments { get;set;}
        public int StatusId { get; set; }
        public bool IsActive { get;set;}
        public DateTime CreatedOn { get;set;}
    }
}
