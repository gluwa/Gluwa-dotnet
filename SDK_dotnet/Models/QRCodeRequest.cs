namespace Gluwa.SDK_dotnet.Models
{
    public class QRCodeRequest : QRCodeRequestBase
    {
        /// <summary>
        /// Your API Key.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Your API Secret.
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Your public address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Your Private Key.
        /// </summary>
        public string PrivateKey { get; set; }

        /// <summary>
        /// Desired image format, optional. Defaults to base64 string
        /// </summary>
        public string Format { get; set; }
    }
}