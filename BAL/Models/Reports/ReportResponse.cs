using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models.Reports
{
    public class ReportResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Errors { get; set; }
        public List<T> ResultData { get; set; }
    }
}
