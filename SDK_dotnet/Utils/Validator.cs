using System;

namespace Gluwa.SDK_dotnet.Utils
{
    internal static class Validator
    {
        public static bool TryValidate(string param)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                throw new ArgumentNullException(nameof(param));
            }

            return true;
        }
    }
}
