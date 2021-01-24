using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gluwa.SDK_dotnet.Models.Exchange
{
    public sealed class CreateOrderRequest
    {
        /// <summary>
        /// Conversion symbol for the order. 
        /// </summary>
        [Required]
        public EConversion? Conversion { get; set; }

        /// <summary>
        /// The address that funds the source amount.
        /// </summary>
        [Required]
        public string SendingAddress { get; set; }

        /// <summary>
        /// The address that the exchanged amount will be received.
        /// </summary>
        [Required]
        public string ReceivingAddress { get; set; }

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

        public IEnumerable<ValidationResult> Validate()
        {
            if (!Conversion.HasValue)
            {
                yield return new ValidationResult($"{nameof(Conversion)} parameter is required.", new[] { nameof(Conversion) });
            }
            else if (string.IsNullOrWhiteSpace(SendingAddress))
            {
                yield return new ValidationResult($"{nameof(SendingAddress)} parameter is required.", new[] { nameof(SendingAddress) });
            }
            else if (string.IsNullOrWhiteSpace(ReceivingAddress))
            {
                yield return new ValidationResult($"{nameof(ReceivingAddress)} parameter is required.", new[] { nameof(ReceivingAddress) });
            }
            else if (string.IsNullOrWhiteSpace(SourceAmount))
            {
                yield return new ValidationResult($"{nameof(SourceAmount)} parameter is required.", new[] { nameof(SourceAmount) });
            }
            else if (string.IsNullOrWhiteSpace(Price))
            {
                yield return new ValidationResult($"{nameof(Price)} parameter is required.", new[] { nameof(Price) });
            }
        }
    }
}
