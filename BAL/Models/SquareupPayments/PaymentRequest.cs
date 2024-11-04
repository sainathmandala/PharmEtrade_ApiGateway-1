namespace BAL.Models.SquareupPayments
{
    public class SquareupPaymentRequest
    {
        public string SourceId { get; set; }

        /// <summary>
        /// The amount to be charged in the smallest currency unit (e.g., cents for USD).
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Optional currency code (e.g., USD). Defaults can be set if needed.
        /// </summary>
        public string Currency { get; set; } = "USD";

        /// <summary>
        /// An optional note or reference ID for the transaction.
        /// </summary>
        public string Note { get; set; }
    }
}
