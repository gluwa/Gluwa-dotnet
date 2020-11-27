using System.Collections.Generic;

namespace Gluwa.SDK_dotnet.Models.Exchange
{
    public sealed class GetOrderResponse
    {
        /// <summary>
        /// The order ID.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Conversion symbol for the order.
        /// </summary>
        public string Conversion { get; set; }

        /// <summary>
        /// The address where the source amount is sent from.
        /// </summary>
        public string SendingAddress { get; set; }

        /// <summary>
        /// The amount available for exchange in source currency.
        /// </summary>
        public string SourceAmount { get; set; }

        /// <summary>
        /// The price the order will use for the exchange.
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// The address where the exchanged amount is received.
        /// </summary>
        public string ReceivingAddress { get; set; }

        /// <summary>
        /// Active, Complete or Canceled.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The list of the past and pending exchanges this order is fulfilling.
        /// </summary>
        public List<Exchange> Exchanges { get; set; }
    }
}
