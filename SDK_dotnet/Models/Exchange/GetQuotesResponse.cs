using System;

namespace Gluwa.SDK_dotnet.Models.Exchange
{
    public sealed class GetQuotesResponse
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
        /// The estimated exchange amount. Sometimes, if someone takes the order before you, the exchange will not be executed.
        /// </summary>
        public string EstimatedExchangedAmount { get; set; }

        /// <summary>
        /// The average of all the prices in the list of matched orders.
        /// </summary>
        public string AveragePrice { get; set; }

        /// <summary>
        /// The best price available in the list of matched orders.
        /// </summary>
        public string BestPrice { get; set; }

        /// <summary>
        /// The worst price available in the list of matched orders. 
        /// </summary>
        public string WorstPrice { get; set; }

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
    }
}
