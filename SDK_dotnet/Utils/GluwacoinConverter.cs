using Gluwa.SDK_dotnet.Models;
using Nethereum.Util;
using System.Numerics;

namespace Gluwa.SDK_dotnet.Utils
{
    internal static class GluwacoinConverter
    {
        private const int SUSDCG_DECIMALS = 6;
        private const int SNGNG_DECIMALS = 18;

        public static BigInteger ConvertToGluwacoinBigInteger(string amount)
        {
            BigDecimal bigDecimalAmount = BigDecimal.Parse(amount);
            return BigInteger.Parse((bigDecimalAmount * new BigDecimal(1, 18)).Floor().ToString());
        }

        public static BigInteger ConvertToGluwacoinSideChainBigInteger(string amount, ECurrency currency)
        {
            int decimals = 0;
            if (currency == ECurrency.sUSDCG)
            {
                decimals = SUSDCG_DECIMALS;
            }
            else if (currency == ECurrency.sNGNG)
            {
                decimals = SNGNG_DECIMALS;
            }

            BigDecimal bigDecimalAmount = BigDecimal.Parse(amount);
            return BigInteger.Parse((bigDecimalAmount * new BigDecimal(1, decimals)).Floor().ToString());
        }
    }
}