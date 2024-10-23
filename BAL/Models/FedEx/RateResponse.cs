namespace BAL.Models.FedEx.RateResponse
{
    public class Alert
    {
        public string code { get; set; }
        public string message { get; set; }
        public string alertType { get; set; }
    }

    public class CurrencyExchangeRate
    {
        public string fromCurrency { get; set; }
        public string intoCurrency { get; set; }
        public double rate { get; set; }
    }

    public class CustomerMessage
    {
        public string code { get; set; }
        public string message { get; set; }
    }

    public class Name
    {
        public string type { get; set; }
        public string encoding { get; set; }
        public string value { get; set; }
    }

    public class OperationalDetail
    {
        public bool ineligibleForMoneyBackGuarantee { get; set; }
        public string astraDescription { get; set; }
        public string airportId { get; set; }
        public string serviceCode { get; set; }
    }

    public class Output
    {
        public List<Alert> alerts { get; set; }
        public List<RateReplyDetail> rateReplyDetails { get; set; }
        public string quoteDate { get; set; }
        public bool encoded { get; set; }
    }

    public class RatedShipmentDetail
    {
        public string rateType { get; set; }
        public string ratedWeightMethod { get; set; }
        public double totalDiscounts { get; set; }
        public double totalBaseCharge { get; set; }
        public double totalNetCharge { get; set; }
        public double totalVatCharge { get; set; }
        public double totalNetFedExCharge { get; set; }
        public double totalDutiesAndTaxes { get; set; }
        public double totalNetChargeWithDutiesAndTaxes { get; set; }
        public double totalDutiesTaxesAndFees { get; set; }
        public double totalAncillaryFeesAndTaxes { get; set; }
        public ShipmentRateDetail shipmentRateDetail { get; set; }
        public string currency { get; set; }
    }

    public class RateReplyDetail
    {
        public string serviceType { get; set; }
        public string serviceName { get; set; }
        public string packagingType { get; set; }
        public List<CustomerMessage> customerMessages { get; set; }
        public List<RatedShipmentDetail> ratedShipmentDetails { get; set; }
        public OperationalDetail operationalDetail { get; set; }
        public string signatureOptionType { get; set; }
        public ServiceDescription serviceDescription { get; set; }
    }

    public class RateResponse
    {
        public string transactionId { get; set; }
        public string customerTransactionId { get; set; }
        public Output output { get; set; }
    }

    public class ServiceDescription
    {
        public string serviceId { get; set; }
        public string serviceType { get; set; }
        public string code { get; set; }
        public List<Name> names { get; set; }
        public string serviceCategory { get; set; }
        public string description { get; set; }
        public string astraDescription { get; set; }
    }

    public class ShipmentRateDetail
    {
        public string rateZone { get; set; }
        public int dimDivisor { get; set; }
        public double fuelSurchargePercent { get; set; }
        public double totalSurcharges { get; set; }
        public double totalFreightDiscount { get; set; }
        public List<SurCharge> surCharges { get; set; }
        public string pricingCode { get; set; }
        public CurrencyExchangeRate currencyExchangeRate { get; set; }
        public TotalBillingWeight totalBillingWeight { get; set; }
        public string dimDivisorType { get; set; }
        public string currency { get; set; }
        public string rateScale { get; set; }
        public TotalRateScaleWeight totalRateScaleWeight { get; set; }
    }

    public class SurCharge
    {
        public string type { get; set; }
        public string description { get; set; }
        public double amount { get; set; }
    }

    public class TotalBillingWeight
    {
        public string units { get; set; }
        public double value { get; set; }
    }

    public class TotalRateScaleWeight
    {
        public string units { get; set; }
        public double value { get; set; }
    }


}
