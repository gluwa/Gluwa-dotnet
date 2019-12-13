using Nethereum.Util;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Gluwa.Utils
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