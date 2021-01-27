using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Gluwa.SDK_dotnet.Models.Exchange
{
    internal sealed class PatchExchangeRequest
    {
        /// <summary>
        /// The address that funds the source amount. Must be the same value as the SendingAddress you used when creating a new order.
        /// </summary>
        [Required]
        public string SendingAddress { get; set; }

        /// <summary>
        /// Reserve transaction signature used to reserve funds for the exchange.
        /// </summary>
        [Required]
        public string ReserveTxnSignature { get; set; }

        /// <summary>
        /// Optional. Required if the source currency is Gluwacoin currency.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Nonce { get; set; }

        /// <summary>
        /// Optional. Required if the source currency BTC. 
        /// Execute transaction signature used to execute the exchange when your funds are available in the reserve address.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ExecuteTxnSignature { get; set; }

        /// <summary>
        /// Optional. Required if the source currency BTC. 
        /// Reclaim transaction signature used to return the funds in the reserve address when the exchange fails or expires.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ReclaimTxnSignature { get; set; }
    }
}
