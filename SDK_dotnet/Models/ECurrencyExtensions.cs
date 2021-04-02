using System;

namespace Gluwa.SDK_dotnet.Models
{
    internal static class ECurrencyExtensions
    {
        internal static bool IsGluwaCoinCurrency(this ECurrency currency)
        {
            switch (currency)
            {
                case ECurrency.KRWG:
                case ECurrency.USDG:
                case ECurrency.NGNG:
                case ECurrency.sUSDCG:
                case ECurrency.sNGNG:
                    return true;

                case ECurrency.BTC:
                    return false;

                default:
                    throw new ArgumentOutOfRangeException($"Unsupported currecny: {currency}");
            }
        }

        internal static bool IsGluwacoinSideChainCurrency(this ECurrency currency)
        {
            switch (currency)
            {
                case ECurrency.sUSDCG:
                case ECurrency.sNGNG:
                    return true;

                case ECurrency.KRWG:
                case ECurrency.USDG:
                case ECurrency.NGNG:
                case ECurrency.BTC:
                    return false;

                default:
                    throw new ArgumentOutOfRangeException($"Unsupported currecny: {currency}");
            }
        }

        internal static bool IsGluwaExchangeCurrency(this ECurrency currency)
        {
            switch (currency)
            {
                case ECurrency.KRWG:
                case ECurrency.USDG:
                case ECurrency.BTC:
                case ECurrency.sUSDCG:
                    return true;

                case ECurrency.NGNG:
                case ECurrency.sNGNG:
                    return false;

                default:
                    throw new ArgumentOutOfRangeException($"Unsupported currecny: {currency}");
            }
        }
    }
}
