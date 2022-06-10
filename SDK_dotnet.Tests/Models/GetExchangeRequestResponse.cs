using Newtonsoft.Json;

namespace Gluwa.SDK_dotnet.Tests.Models
{
    public sealed class GetExchangeRequestResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public string Conversion { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string DestinationAddress { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string SourceAmount { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Fee { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Executor { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string ExpiryBlockNumber { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string ReservedFundsAddress { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string ReservedFundsRedeemScript { get; private set; }


        [JsonConstructor]
        public GetExchangeRequestResponse(
            string conversion,
            string destinationAddress,
            string id,
            string sourceAmount,
            string fee,
            string executor,
            string expiryBlockNumber,
            string reservedFundsAddress,
            string reservedFundsRedeemScript)
        {
            Conversion = conversion;
            DestinationAddress = destinationAddress;
            ID = id;
            SourceAmount = sourceAmount;
            Fee = fee;
            Executor = executor;
            ExpiryBlockNumber = expiryBlockNumber;
            ReservedFundsAddress = reservedFundsAddress;
            ReservedFundsRedeemScript = reservedFundsRedeemScript;
        }
    }
}
