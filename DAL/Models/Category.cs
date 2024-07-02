using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Category
    {
        public Category()
        {
            AddProducts = new HashSet<AddProduct>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

        public virtual ICollection<AddProduct> AddProducts { get; set; }
    }
}
