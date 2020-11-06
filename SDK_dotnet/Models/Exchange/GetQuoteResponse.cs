using System;
using System.Collections.Generic;

namespace Gluwa.SDK_dotnet.Models.Exchange
{
    public sealed class GetQuoteResponse
    {
        /// <summary>
        /// Accepted quote ID.
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// The address that funded the source amount.
        /// </summary>
        public string SendingAddress { get; set; }

        /// <summary>
        /// The total amount.
        /// </summary>
        public string SourceAmount { get; set; }

        /// <summary>
        /// The total fee of the exchange.
        /// </summary>
        public string Fee { get; set; }

        /// <summary>
        /// The estimated exchange amount. Sometimes, if someone takes the order before you, the exchange will not be executed.
        /// </summary>
        public string EstimatedExchangedAmount { get; set; }

        /// <summary>
        /// The address that the exchanged currency is received.
        /// </summary>
        public string ReceivingAddress { get; set; }

        /// <summary>
        /// Pending or Processed.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The conversion of the quote.
        /// </summary>
        public string Conversion { get; set; }

        /// <summary>
        /// The list of matched orders that fulfilled the source amount. 
        /// </summary>
        public List<MatchedOrder> MatchedOrders { get; set; }
    }
}
