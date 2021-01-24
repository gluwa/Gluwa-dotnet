using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gluwa.SDK_dotnet.Models.Exchange
{
    public sealed class GetOrdersOptions
    {
        /// <summary>
        /// datetime. If defined, only orders created after this datetime are included in the response.
        /// </summary>
        public DateTime? StartDateTime { get; set; }

        /// <summary>
        /// datetime. If defined, only orders created before this datetime are included in the response.
        /// </summary>
        public DateTime? EndDateTime { get; set; }

        /// <summary>
        /// Active, Complete or Canceled. If specified, only orders with the specified status will be included in the response.
        /// </summary>
        public EOrderStatus? Status { get; set; }

        /// <summary>
        /// Number of orders to skip the beginning of list. Defaults to 0.
        /// </summary>
        public int Offset { get; set; } = 0;

        /// <summary>
        /// Number of orders to include in the result. Defaults to 25, maximum of 100.
        /// </summary>
        public int Limit { get; set; } = 25;

        public IEnumerable<ValidationResult> Validate()
        {
            if (Limit > 100)
            {
                yield return new ValidationResult($"The maximum value of the {nameof(Limit)} is 100.", new[] { nameof(Limit) });
            }
        }
    }
}
