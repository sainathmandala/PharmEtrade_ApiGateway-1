using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models.FedEx
{
    public class AddressValidationResponse
    {
        public string transactionId { get; set; }
        public AddressValidationOutput output { get; set; }
    }

    public class AddressValidationOutput
    {
        public List<ResolvedAddress> resolvedAddresses { get; set; }
    }

    public class ResolvedAddress
    {
        public string clientReferenceId { get; set; }
        public List<string> streetLinesToken { get; set; }
        public List<CityToken> cityToken { get; set; }
        public string stateOrProvinceCode { get; set; }
        public StateOrProvinceCodeToken stateOrProvinceCodeToken { get; set; }
        public PostalCodeToken postalCodeToken { get; set; }
        public string countryCode { get; set; }
        public AddressAttributes attributes { get; set; }
    }

    public class CityToken
    {
        public bool changed { get; set; }
        public string value { get; set; }
    }

    public class StateOrProvinceCodeToken
    {
        public bool changed { get; set; }
        public string value { get; set; }
    }

    public class PostalCodeToken
    {
        public bool changed { get; set; }
        public string value { get; set; }
    }

    public class AddressAttributes
    {
        public string POBox { get; set; }
        public string POBoxOnlyZIP { get; set; }
        public string SplitZIP { get; set; }
        public string SuiteRequiredButMissing { get; set; }
        public string InvalidSuiteNumber { get; set; }
        public string ResolutionInput { get; set; }
        public string DPV { get; set; }
        public string ResolutionMethod { get; set; }
        public string MatchSource { get; set; }
        public string CountrySupported { get; set; }
        public string ValidlyFormed { get; set; }
        public string Matched { get; set; }
        public string Resolved { get; set; }
        public string AddressType { get; set; }
        public string AddressPrecision { get; set; }
        public string MultipleMatches { get; set; }
    }
}
