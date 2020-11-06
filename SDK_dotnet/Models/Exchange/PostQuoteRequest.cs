using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Gluwa.SDK_dotnet.Models.Exchange
{
    internal sealed class PostQuoteRequest
    {
        /// <summary>
        /// The amount in source currency you want to exchange.
        /// </summary>
        [Required]
        public string Amount { get; set; }

        /// <summary>
        /// Conversion symbol for the exchange.
        /// </summary>
        [Required]
        public EConversion? Conversion { get; set; }

        /// <summary>
        /// The address that will fund the source amount.
        /// </summary>
        [Required]
        public string SendingAddress { get; set; }

        /// <summary>
        /// The signature of the sending address. 
        /// </summary>
        [Required]
        public string SendingAddressSignature { get; set; }

        /// <summary>
        /// The address that the exchanged currency will be received.
        /// </summary>
        [Required]
        public string ReceivingAddress { get; set; }

        /// <summary>
        /// The signature of the receiving address.
        /// </summary>
        [Required]
        public string ReceivingAddressSignature { get; set; }

        /// <summary>
        /// Optional. Required if the source currency is BTC. 
        /// This is BTC public key of the sending address that will be used to reserve your funds.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BtcPublicKey { get; set; }
    }
}
