using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ResponseModels
{
    public class UploadResponse
    {
        public int Status { get; set; }
        public string ImageUrl { get; set; }
        public string Message { get; set; }
    }
}
