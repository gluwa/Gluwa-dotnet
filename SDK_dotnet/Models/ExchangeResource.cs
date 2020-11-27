using Gluwa.SDK_dotnet.Models.Exchange;

namespace Gluwa.SDK_dotnet.Models
{
    public sealed class ExchangeResource : IResourceObj
    {
        /// <summary>
        /// The ID of the order.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Amount remaining in the order.
        /// </summary>
        public string OrderAmountRemaining { get; set; }

        /// <summary>
        /// Conversion symbol for the order.
        /// </summary>
        public EConversion Conversion { get; set; }

        /// <summary>
        /// The address where the source amount was sent from.
        /// </summary>
        public string SendingAddress { get; set; }

        /// <summary>
        /// The address where the exchanged amount was received.
        /// </summary>
        public string ReceivingAddress { get; set; }

        /// <summary>
        /// The amount that was sent in source currency.
        /// </summary>
        public string SourceAmount { get; set; }

        /// <summary>
        /// The price used for the exchange.
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// The amount that was received in exchanged currency.
        /// </summary>
        public string ExchangedAmount { get; set; }
    }
}
