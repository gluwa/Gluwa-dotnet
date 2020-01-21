using Nethereum.Util;
using System.Numerics;

namespace Gluwa.SDK_dotnet.Utils
{
    internal static class GluwacoinConverter
    {
        public static BigInteger ConvertToGluwacoinBigInteger(string amount)
        {
            BigDecimal bigDecimalAmount = BigDecimal.Parse(amount);
            return BigInteger.Parse((bigDecimalAmount * new BigDecimal(1, 18)).Floor().ToString());
        }
    }
}