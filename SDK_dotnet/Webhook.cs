using Gluwa.SDK_dotnet.Models;
using Gluwa.SDK_dotnet.Utils;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Gluwa.SDK_dotnet
{
    /// <summary>
    /// Verify that the webhook is sent by the Gluwa.
    /// </summary>
    public static class Webhook
    {
        /// <summary>
        /// Verify the requested Signature and Payload(Version 1)
        /// </summary>
        /// <param name="payLoad">Payload</param>
        /// <param name="signature">The value of X-REQUEST-SIGNATURE</param>
        /// <param name="webhookSecretKey">Your Webhook Secret.</param>
        public static bool ValidateWebhook(PayLoad payLoad, string signature, string webhookSecretKey)
        {
            string payload = Converter.ToJson(payLoad);

            if (string.IsNullOrWhiteSpace(payload))
            {
                throw new ArgumentNullException(nameof(payload));
            }
            else if (string.IsNullOrWhiteSpace(signature))
            {
                throw new ArgumentNullException(nameof(signature));
            }

            string payloadHashBase64;
            byte[] key = Encoding.UTF8.GetBytes(webhookSecretKey);
            using (var encryptor = new HMACSHA256(key))
            {
                byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);
                byte[] payloadHashedBytes = encryptor.ComputeHash(payloadBytes);
                payloadHashBase64 = Convert.ToBase64String(payloadHashedBytes);
            }

            return payloadHashBase64 == signature;
        }

        /// <summary>
        /// Verify the requested Signature and Payload(Version 2)
        /// </summary>
        /// <param name="payLoad">Payload</param>
        /// <param name="signature">The value of X-REQUEST-SIGNATURE</param>
        /// <param name="webhookSecretKey">Your Webhook Secret.</param>
        public static bool ValidateWebhookV2(PayLoadV2 payLoad, string signature, string webhookSecretKey)
        {
            string payload = Converter.ToJson(payLoad);

            if (string.IsNullOrWhiteSpace(payload))
            {
                throw new ArgumentNullException(nameof(payload));
            }
            else if (string.IsNullOrWhiteSpace(signature))
            {
                throw new ArgumentNullException(nameof(signature));
            }

            string payloadHashBase64;
            byte[] key = Encoding.UTF8.GetBytes(webhookSecretKey);
            using (var encryptor = new HMACSHA256(key))
            {
                byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);
                byte[] payloadHashedBytes = encryptor.ComputeHash(payloadBytes);
                payloadHashBase64 = Convert.ToBase64String(payloadHashedBytes);
            }

            return payloadHashBase64 == signature;
        }
    }
}