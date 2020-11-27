using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Gluwa.SDK_dotnet.Models.Exchange
{
    internal sealed class PostOrderRequest
    {
        /// <summary>
        /// Conversion symbol for the order. 
        /// </summary>
        [Required]
        public string Conversion { get; set; }

        /// <summary>
        /// The address that funds the source amount.
        /// </summary>
        [Required]
        public string SendingAddress { get; set; }

        /// <summary>
        /// Address Signature of the SendingAddress. 
        /// </summary>
        [Required]
        public string SendingAddressSignature { get; set; }

        /// <summary>
        /// The address that the exchanged amount will be received.
        /// </summary>
        [Required]
        public string ReceivingAddress { get; set; }

        /// <summary>
        /// Address Signature of the ReceivingAddress.
        /// </summary>
        [Required]
        public string ReceivingAddressSignature { get; set; }

        /// <summary>
        /// The amount in source currency to be exchanged.
        /// </summary>
        [Required]
        public string SourceAmount { get; set; }

        /// <summary>
        /// The price you want to use. Any exchange that this order will fulfill will use this price.
        /// </summary>
        [Required]
        public string Price { get; set; }

        /// <summary>
        /// Optional. Required if the source currency is BTC.
        /// The public key for the sending address. 
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BtcPublicKey { get; set; }
    }
}
