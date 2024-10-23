namespace BAL.Models.FedEx.RateRequest
{    
    public class AccountNumber
    {
        public string value { get; set; }
    }

    public class Address
    {
        public string postalCode { get; set; }
        public string countryCode { get; set; }
    }

    public class Recipient
    {
        public Address address { get; set; }
    }

    public class RequestedPackageLineItem
    {
        public Weight weight { get; set; }
    }

    public class RequestedShipment
    {
        public Shipper shipper { get; set; }
        public Recipient recipient { get; set; }
        public string pickupType { get; set; }
        public List<string> rateRequestType { get; set; }
        public List<RequestedPackageLineItem> requestedPackageLineItems { get; set; }
    }

    public class RateRequest
    {
        public AccountNumber accountNumber { get; set; }
        public RequestedShipment requestedShipment { get; set; }
    }

    public class Shipper
    {
        public Address address { get; set; }
    }

    public class Weight
    {
        public string units { get; set; }
        public int value { get; set; }
    }

}
