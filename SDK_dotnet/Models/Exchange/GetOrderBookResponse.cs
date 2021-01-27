namespace Gluwa.SDK_dotnet.Models.Exchange
{
    public sealed class GetOrderBookResponse
    {
        /// <summary>
        /// Amount of available in the order.
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// Price. The unit is <exchanged currency>/<source currency>.
        /// </summary>
        public string Price { get; set; }
    }
}
