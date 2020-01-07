using System.ComponentModel.DataAnnotations;

namespace Gluwa.Models
{
    public sealed class FeeResponse
    {
        /// <summary>
        /// Currency of the fee.
        /// </summary>
        [Required]
        public ECurrency? Currency { get; set; }

        /// <summary>
        /// Current minimum amount of the fee that Gluwa will accept.
        /// </summary>
        [Required]
        public string MinimumFee { get; set; }
    }
}
