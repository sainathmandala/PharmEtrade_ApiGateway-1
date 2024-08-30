using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class NDCUPC
    {
        public int Id { get; set; }
        public string? NDC { get; set; }
        public string? UPC { get; set; }
        public string? ProductName { get; set; }
        public string? ManufacturerName { get; set; }
        public string? Size { get; set; }
        public string? UnitOfMeasurement { get; set; }
        public string? Form { get; set; }
    }
}
