using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class ProductGallery
    {
        public string ProductGalleryId { get; set; }
        public string ProductId { get; set; }
        public string Caption { get; set; }
        public string ImageUrl { get; set; }
        public string Thumbnail1 { get; set; }
        public string Thumbnail2 { get; set; }
        public string Thumbnail3 { get; set; }
        public string Thumbnail4 { get; set; }
        public string Thumbnail5 { get; set; }
        public string Thumbnail6 { get; set; }
        public string VideoUrl { get; set; }
    }
}
