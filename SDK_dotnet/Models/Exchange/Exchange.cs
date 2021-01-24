namespace Gluwa.SDK_dotnet.Models.Exchange
{
    public sealed class Exchange
    {
        /// <summary>
        /// The address where the source amount is sent from.
        /// </summary>
        public string SendingAddress { get; set; }

        /// <summary>
        /// The address where the exchanged amount is received.
        /// </summary>
        public string ReceivingAddress { get; set; }

        /// <summary>
        /// The amount in source currency to be exchanged.
        /// </summary>
        public string SourceAmount { get; set; }

        /// <summary>
        /// The total fee paid for the exchange.
        /// </summary>
        public string Fee { get; set; }

        /// <summary>
        /// The amount in exchanged currency you received.
        /// </summary>
        public string ExchangedAmount { get; set; }

        /// <summary>
        /// The price used for this exchange.
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// Pending, Success or Failed.
        /// </summary>
        public string Status { get; set; }
    }
}
