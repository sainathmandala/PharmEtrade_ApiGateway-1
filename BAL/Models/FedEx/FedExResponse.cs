namespace BAL.Models.FedEx
{
    public class FedExResponse
    {
        public string transactionId { get; set; } // Changed to lowercase
        public string customerTransactionId { get; set; }
        public Output output { get; set; }
    }

    //public class Output
    //{
    //    public List<TransactionShipment> transactionShipments { get; set; }
    //}

    public class TransactionShipment
    {
        public string masterTrackingNumber { get; set; }
        public string serviceType { get; set; }
        public string shipDatestamp { get; set; }
        public string serviceName { get; set; }
        public List<PieceResponse> pieceResponses { get; set; }
        public CompletedShipmentDetail completedShipmentDetail { get; set; }
        public string serviceCategory { get; set; }
    }

    public class PieceResponse
    {
        public string masterTrackingNumber { get; set; }
        public string trackingNumber { get; set; }
        public string deliveryDatestamp { get; set; }
        public List<PackageDocument> packageDocuments { get; set; }
    }

    public class PackageDocument
    {
        public string url { get; set; }
        public string contentType { get; set; }
        public int copiesToPrint { get; set; }
        public string docType { get; set; }
    }

    public class CompletedShipmentDetail
    {
        public bool usDomestic { get; set; }
        public string carrierCode { get; set; }
        public MasterTrackingId masterTrackingId { get; set; }
        public OperationalDetail operationalDetail { get; set; }
        public string packagingDescription { get; set; }
    }

    public class MasterTrackingId
    {
        public string trackingIdType { get; set; }
        public string formId { get; set; }
        public string trackingNumber { get; set; }
    }

    public class OperationalDetail
    {
        public string deliveryDate { get; set; }
        public string destinationLocationStateOrProvinceCode { get; set; }
        public string postalCode { get; set; }
        public string countryCode { get; set; }
        public string serviceCode { get; set; }
    }
}
