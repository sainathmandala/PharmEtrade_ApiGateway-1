using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class AddtoCartproduct
    {
        public int AddtoCartId { get; set; }
        public int Userid { get; set; }
        public int? Imageid { get; set; }
        public int ProductId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
