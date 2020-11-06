using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gluwa.SDK_dotnet.Models.Exchange
{
    public sealed class MatchedOrderForQuote
    {
        /// <summary>
        /// Order ID
        /// </summary>
        public Guid? OrderID { get; set; }

        /// <summary>
        /// The address where the source amount must be sent to.
        /// </summary>
        public string DestinationAddress { get; set; }

        /// <summary>
        /// The amount in source currency that this order will exchange.
        /// </summary>
        public string SourceAmount { get; set; }

        /// <summary>
        /// The fee amount that must be used to create ReserveTxnSignature, ExecuteTxnSignature and ReclaimTxnSignature
        /// </summary>
        public string Fee { get; set; }

        /// <summary>
        /// Optional. Included only when the source currency is a Gluwacoin currency.
        /// Block number at which the reserve transaction should expire.
        /// </summary> 
        public string ExpiryBlockNumber { get; set; }

        /// <summary>
        /// Optional. Included only when the source currency is a Gluwacoin currency.
        /// Gluwa address that will execute the exchange on the user's behalf.
        /// </summary>        
        public string Executor { get; set; }

        /// <summary>
        /// Optional. Required if the source currency is BTC.
        /// </summary>
        public string ReservedFundsAddress { get; set; }

        /// <summary>
        /// Optional. Required if the source currency is BTC. 
        /// </summary>
        public string ReservedFundsRedeemScript { get; set; }

        public IEnumerable<ValidationResult> Validate(ECurrency currency)
        {
            if (OrderID == null || OrderID == Guid.Empty)
            {
                yield return new ValidationResult($"{nameof(OrderID)} parameter is required.", new[] { nameof(OrderID) });
            }
            else if (string.IsNullOrWhiteSpace(DestinationAddress))
            {
                yield return new ValidationResult($"{nameof(DestinationAddress)} parameter is required.", new[] { nameof(DestinationAddress) });
            }
            else if (string.IsNullOrWhiteSpace(SourceAmount))
            {
                yield return new ValidationResult($"{nameof(SourceAmount)} parameter is required.", new[] { nameof(SourceAmount) });
            }
            else if (string.IsNullOrWhiteSpace(Fee))
            {
                yield return new ValidationResult($"{nameof(Fee)} parameter is required.", new[] { nameof(Fee) });
            }

            if (currency == ECurrency.BTC)
            {
                if (string.IsNullOrWhiteSpace(ReservedFundsAddress))
                {
                    yield return new ValidationResult($"{nameof(ReservedFundsAddress)} parameter is required for {currency}.", new[] { nameof(ReservedFundsAddress) });
                }
                else if (string.IsNullOrWhiteSpace(ReservedFundsRedeemScript))
                {
                    yield return new ValidationResult($"{nameof(ReservedFundsRedeemScript)} parameter is required for {currency}.", new[] { nameof(ReservedFundsRedeemScript) });
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(ExpiryBlockNumber))
                {
                    yield return new ValidationResult($"{nameof(ExpiryBlockNumber)} parameter is required for {currency}.", new[] { nameof(ExpiryBlockNumber) });
                }
                else if (string.IsNullOrWhiteSpace((Executor)))
                {
                    yield return new ValidationResult($"{nameof(Executor)} parameter is required for {currency}.", new[] { nameof(Executor) });
                }
            }
        }
    }
}
