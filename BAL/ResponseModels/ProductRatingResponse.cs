using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ResponseModels
{
    public class ProductRatingResponse
    {
        public string RatingID { get; set; }
        public string ProductID { get; set; }
        public string CustomerID { get; set; }
        public string Rating { get; set; }
        public string Feedback { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }

    }
}
