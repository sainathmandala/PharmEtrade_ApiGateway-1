namespace BAL.Models.FedEx
{
    //public class PostalValidationErrorResponse
    //{
    //    public string transactionId { get; set; }
    //    public List<PostalValidationError> errors { get; set; }
    //    public PostalValidationOutput output { get; set; }
    //}

    //public class PostalValidationError
    //{
    //    public string code { get; set; }
    //    public string message { get; set; }
    //}

    //public class PostalValidationOutput
    //{
    //    public List<Alert> alerts { get; set; }
    //    public string cleanedPostalCode { get; set; }
    //    public string countryCode { get; set; }
    //    public string stateOrProvinceCode { get; set; }
    //    public List<LocationDescription> locationDescriptions { get; set; }
    //}

    public class Alert
    {
        public string code { get; set; }
        public string message { get; set; }
        public string alertType { get; set; }
    }

    //public class LocationDescription
    //{
    //    public string locationId { get; set; }
    //    public int locationNumber { get; set; }
    //    public string serviceArea { get; set; }
    //    public string airportId { get; set; }
    //}
}
