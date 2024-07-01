using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class ProductGallery
    {
        public ProductGallery()
        {
            AddProducts = new HashSet<AddProduct>();
        }

        public int GalleryId { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string? Caption { get; set; }

        public virtual ICollection<AddProduct> AddProducts { get; set; }
    }
}
