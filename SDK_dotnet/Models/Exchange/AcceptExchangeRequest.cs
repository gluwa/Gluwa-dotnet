using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Gluwa.SDK_dotnet.Models.Exchange
{
    public sealed class AcceptExchangeRequest
    {
        /// <summary>
        /// Excahnge Request ID
        /// </summary>
        public Guid? ID { get; set; }

        /// <summary>
        /// Conversion symbol for the exchange.
        /// </summary>
        public EConversion? Conversion { get; set; }

        /// <summary>
        /// The address where the source amount must be sent to.
        /// </summary>
        public string DestinationAddress { get; set; }

        /// <summary>
        /// The amount in source currency.
        /// </summary>
        public string SourceAmount { get; set; }

        /// <summary>
        /// The fee amount that must be used to create ReserveTxnSignature, ExecuteTxnSignature and ReclaimTxnSignature
        /// </summary>
        public string Fee { get; set; }

        /// <summary>
        /// Optional. Included only when the source currency is a Gluwacoin currency.
        /// </summary>
        public string Executor { get; set; }

        /// <summary>
        /// Optional. Included only when the source currency is a Gluwacoin currency.
        /// </summary>
        public BigInteger? ExpiryBlockNumber { get; set; }

        /// <summary>
        /// Optional. Required if the source currency is BTC. 
        /// </summary>
        public string ReservedFundsAddress { get; set; }

        /// <summary>
        /// Optional. Required if the source currency is BTC. 
        /// </summary>
        public string ReservedFundsRedeemScript { get; set; }

        public IEnumerable<ValidationResult> Validate()
        {
            if (ID == null || ID == Guid.Empty)
            {
                yield return new ValidationResult($"{nameof(ID)} parameter is required.", new[] { nameof(ID) });
            }
            else if (Conversion == null)
            {
                yield return new ValidationResult($"{nameof(Conversion)} parameter is required.", new[] { nameof(Conversion) });
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

            if (Conversion.Value.IsSourceCurrencyBtc())
            {
                if (string.IsNullOrWhiteSpace(ReservedFundsAddress))
                {
                    yield return new ValidationResult($"{nameof(ReservedFundsAddress)} parameter is required for source currency({Conversion.Value.ToSourceCurrency()}).", new[] { nameof(ReservedFundsAddress) });

                }
                else if (string.IsNullOrWhiteSpace(ReservedFundsRedeemScript))
                {
                    yield return new ValidationResult($"{nameof(ReservedFundsRedeemScript)} parameter is required for source currency ({Conversion.Value.ToSourceCurrency()}).", new[] { nameof(ReservedFundsRedeemScript) });

                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(Executor))
                {
                    yield return new ValidationResult($"{nameof(Executor)} parameter is required for source currency ({Conversion.Value.ToSourceCurrency()}).", new[] { nameof(Executor) });
                }
                else if (ExpiryBlockNumber == null || ExpiryBlockNumber == BigInteger.Zero)
                {
                    yield return new ValidationResult($"{nameof(ExpiryBlockNumber)} parameter is required for source currency ({Conversion.Value.ToSourceCurrency()}).", new[] { nameof(ExpiryBlockNumber) });
                }
            }
        }
    }
}
