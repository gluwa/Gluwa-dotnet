using System;

namespace Gluwa.SDK_dotnet.Models.Exchange
{
    internal sealed class MatchedOrderRequest
    {
        /// <summary>
        /// ID of the order that was matched
        /// </summary>
        public Guid? OrderID { get; set; }

        /// <summary>
        /// Reserve transaction signature used to reserve funds for the exchange.
        /// </summary>
        public string ReserveTxnSignature { get; set; }

        /// <summary>
        /// Optional. Required if the source currency is Gluwacoin currency.
        /// </summary>
        public string Nonce { get; set; }

        /// <summary>
        /// Optional.Required if the source currency BTC.
        /// </summary>
        public string ExecuteTxnSignature { get; set; }

        /// <summary>
        /// Optional. Required if the source currency BTC.
        /// </summary>
        public string ReclaimTxnSignature { get; set; }
    }
}
