namespace Gluwa.SDK_dotnet.Models.Exchange
{
    public sealed class MatchedOrder
    {
        /// <summary>
        /// The amount in source currency that this order will exchange.
        /// </summary>
        public string SourceAmount { get; set; }

        /// <summary>
        /// The fee amount used to create ReserveTxnSignature, ExecuteTxnSignature and ReclaimTxnSignature
        /// </summary>
        public string Fee { get; set; }

        /// <summary>
        /// Pending, Success or Failed.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The price this order is offering for the exchange. 
        /// </summary>
        public string Price { get; set; }
    }
}
