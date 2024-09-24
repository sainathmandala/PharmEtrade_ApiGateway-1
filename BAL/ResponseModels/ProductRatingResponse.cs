using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ResponseModels
{
    public class ProductRatingResponse
    {
        public string RatingId { get; set; }
        public string ProductId { get; set; }
        public string CustomerId { get; set; }
        public int Rating { get; set; }
        public string Feedback { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }

    }
}
