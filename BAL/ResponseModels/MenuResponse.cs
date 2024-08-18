using BAL.Models;

namespace BAL.ResponseModels
{
    public class MenuResponse
    {        
        public int StatusCode{ get; set; }
        public string Message { get; set; }
        public List<Menu> Result { get; set; }
    }
}
