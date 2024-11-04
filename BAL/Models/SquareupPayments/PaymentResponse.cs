namespace BAL.Models.SquareupPayments
{
    public class SquareupPaymentResponse
    {
        public string Id { get; set; } // Payment ID
        public long? Amount { get; set; } // Amount in cents
        public string Currency { get; set; } // Currency code (e.g., "USD")
        public string Status { get; set; } // Payment status (e.g., "COMPLETED")
        public string ReceiptUrl { get; set; } // URL to the receipt
        public string CardBrand { get; set; } // Credit card brand (e.g., "VISA")
        public string Last4 { get; set; } // Last 4 digits of the card
        public string Expiration { get; set; } // Card expiration date
    }
}
