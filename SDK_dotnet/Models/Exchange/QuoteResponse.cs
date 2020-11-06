using System;
using System.Collections.Generic;

namespace Gluwa.SDK_dotnet.Models.Exchange
{
    public sealed class QuoteResponse
    {
        /// <summary>
        /// The conversion of the exchange.
        /// </summary>
        public EConversion Conversion { get; set; }

        /// <summary>
        /// The total amount in source currency that will be exchanged.
        /// </summary>
        public string TotalSourceAmount { get; set; }

        /// <summary>
        /// The exchange fee.
        /// </summary>
        public string TotalFee { get; set; }

        /// <summary>
        /// The total estimated exchanged amount in exchanged currency.
        /// </summary>
        public string TotalEstimatedExchangedAmount { get; set; }

        /// <summary>
        /// The average of all the prices in the list of matched orders.
        /// </summary>
        public string AveragePrice { get; set; }

        /// <summary>
        /// The best price available in the list of matched orders.
        /// </summary>
        public string BestPrice { get; set; }

        /// <summary>
        /// The best price available in the list of matched orders. 
        /// </summary>
        public string WorstPrice { get; set; }

        /// <summary>
        /// The list of matched orders available to fulfill the requested source amount. 
        /// </summary>
        public List<MatchedOrderResponse> MatchedOrders { get; set; }

        /// <summary>
        /// The time when the quote is created.
        /// </summary>
        public DateTime? CreatedDateTime { get; set; }

        /// <summary>
        /// The duration in seconds that the quote is valid.
        /// </summary>
        public long? TimeToLive { get; set; }

        /// <summary>
        /// Checksum. Used when you accept the quote.
        /// </summary>
        public string Checksum { get; set; }
    }
}
