using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class ProductSize
    {
        public ProductSize()
        {
            AddProducts = new HashSet<AddProduct>();
        }

        public int Sizeid { get; set; }
        public string? Height { get; set; }
        public string? Width { get; set; }
        public string? Length { get; set; }
        public string? Weight { get; set; }

        public virtual ICollection<AddProduct> AddProducts { get; set; }
    }
}
