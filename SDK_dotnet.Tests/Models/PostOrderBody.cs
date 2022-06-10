using System;
[AttributeUsage(AttributeTargets.Property,
      Inherited = false,
      AllowMultiple = false)]
internal sealed class OptionalAttribute : Attribute { }

namespace Gluwa.SDK_dotnet.Tests.Models
{
    public class PostOrderBody
    {
        /// <summary>
        /// Currency conversion according to the order maker. Format: "SourceTarget"
        /// Example: sUsdcgBtc means converting sUsdcg to Btc
        /// </summary>
        public string Conversion { get; set; }

        /// <summary>
        /// The address that funds the source amount
        /// </summary>
        public string SendingAddress { get; set; }

        /// <summary>
        /// Signature of sending address (X-REQUEST-SIGNATURE)
        /// </summary>
        public string SendingAddressSignature { get; set; }

        /// <summary>
        /// The address for the exchanged money
        /// </summary>
        public string ReceivingAddress { get; set; }

        /// <summary>
        /// Signature of receiving address (X-REQUEST-SIGNATURE)
        /// </summary>
        public string ReceivingAddressSignature { get; set; }

        /// <summary>
        /// How much money do you want to sell from source
        /// </summary>
        public string SourceAmount { get; set; }

        /// <summary>
        /// Gas Price for ethereum transaction
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// Only required when source is Btc
        /// </summary>
        [Optional]
        public string BtcPublicKey { get; set; }

    }
}
