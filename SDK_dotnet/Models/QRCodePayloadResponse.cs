namespace Gluwa.SDK_dotnet.Models
{
    public sealed class QRCodePayloadResponse
    {
        public string Base64 { get; set; }

        public QRCodeImagePayload Data { get; set; }
    }
}
