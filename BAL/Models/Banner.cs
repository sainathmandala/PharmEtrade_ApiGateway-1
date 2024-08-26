using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class Banner
    {
        public int BannerId { get; set; }
        public string ImageUrl { get; set; }
        public string BannerText { get; set; }
        public int OrderSequence { get; set; }
        public DateTime UploadedOn { get; set; }
        public int IsActive { get; set; }
    }
}
