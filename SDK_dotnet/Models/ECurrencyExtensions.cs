using System;

namespace Gluwa.SDK_dotnet.Models
{
    internal static class ECurrencyExtensions
    {
        internal static bool IsGluwaCoinCurrency(this ECurrency currency)
        {
            switch (currency)
            {
                case ECurrency.NGNG:
                case ECurrency.sUSDCG:
                case ECurrency.USDCG:
                case ECurrency.sNGNG:
                    return true;

                case ECurrency.BTC:
                    return false;

                default:
                    throw new ArgumentOutOfRangeException($"Unsupported currency: {currency}");
            }
        }

        internal static bool IsGluwacoinSideChainCurrency(this ECurrency currency)
        {
            switch (currency)
            {
                case ECurrency.sUSDCG:
                case ECurrency.sNGNG:
                    return true;

                case ECurrency.NGNG:
                case ECurrency.USDCG:
                case ECurrency.BTC:
                    return false;

                default:
                    throw new ArgumentOutOfRangeException($"Unsupported currency: {currency}");
            }
        }

        internal static bool IsGluwaExchangeCurrency(this ECurrency currency)
        {
            switch (currency)
            {
                case ECurrency.BTC:
                case ECurrency.sUSDCG:
                case ECurrency.USDCG:
                    return true;

                case ECurrency.NGNG:
                case ECurrency.sNGNG:
                    return false;

                default:
                    throw new ArgumentOutOfRangeException($"Unsupported currency: {currency}");
            }
        }
    }
}