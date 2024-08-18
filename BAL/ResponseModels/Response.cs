using BAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ResponseModels
{
    public class Response<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<T> Result { get; set; }
    }
}
