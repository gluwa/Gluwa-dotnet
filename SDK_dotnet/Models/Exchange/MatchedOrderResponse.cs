using System;
using System.ComponentModel.DataAnnotations;

namespace Gluwa.SDK_dotnet.Models.Exchange
{
    public sealed class MatchedOrderResponse
    {
        /// <summary>
        /// Order ID
        /// </summary>
        [Required]
        public Guid? OrderID { get; set; }

        /// <summary>
        /// The address where the source amount must be sent to.
        /// </summary>
        [Required]
        public string DestinationAddress { get; set; }

        /// <summary>
        /// The amount in source currency that this order will exchange.
        /// </summary>
        [Required]
        public string SourceAmount { get; set; }

        /// <summary>
        /// The fee amount that must be used to create ReserveTxnSignature, ExecuteTxnSignature and ReclaimTxnSignature
        /// </summary>
        [Required]
        public string Fee { get; set; }

        /// <summary>
        /// Optional. Included only when the source currency is a Gluwacoin currency.
        /// Block number at which the reserve transaction should expire.
        /// </summary> 
        public string ExpiryBlockNumber { get; set; }

        /// <summary>
        /// Optional. Included only when the source currency is a Gluwacoin currency.
        /// Gluwa address that will execute the exchange on the user's behalf.
        /// </summary>        
        public string Executor { get; set; }

        /// <summary>
        /// Optional. Required if the source currency is BTC.
        /// The address where the source amount must be sent to to reserve your funds for the exchange.
        /// </summary>
        public string ReservedFundsAddress { get; set; }

        /// <summary>
        /// Optional. Required if the source currency is BTC. 
        /// </summary>
        public string ReservedFundsRedeemScript { get; set; }
    }
}
