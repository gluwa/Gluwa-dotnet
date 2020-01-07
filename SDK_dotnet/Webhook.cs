using Gluwa.Models;
using Gluwa.Utils;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Gluwa
{
    /// <summary>
    /// Verify that the webhook is sent by the Gluwa.
    /// </summary>
    public sealed class Webhook
    {
        private readonly string mWebhookSecretKey;

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="webhookSecretKey">Your Webhook Secret.</param>
        public Webhook(string webhookSecretKey)
        {
            mWebhookSecretKey = webhookSecretKey;
        }

        /// <summary>
        /// Verify the requested Signature and Payload
        /// </summary>
        /// <param name="payLoad">Payload</param>
        /// <param name="signature">The value of X-REQUEST-SIGNATURE</param>
        public bool ValidateWebhook(PayLoad payLoad, string signature)
        {
            string payload = Converter.ToJson<PayLoad>(payLoad);

            if (string.IsNullOrWhiteSpace(payload))
            {
                throw new ArgumentNullException(nameof(payload));
            }
            else if (string.IsNullOrWhiteSpace(signature))
            {
                throw new ArgumentNullException(nameof(signature));
            }

            string payloadHashBase64;
            byte[] key = Encoding.UTF8.GetBytes(mWebhookSecretKey);
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