using Gluwa.Models;
using Gluwa.Utils;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Gluwa
{
    /// <summary>
    /// When user completes transfer via the QR code, the Gluwa API sends a webhook to your webhook endpoint.
    /// Verify that the values ​​actually sent by the Gluwa server are correct.
    /// </summary>
    public sealed class Webhook
    {
        private readonly string mWebhookSecretKey;

        /// <summary>
        /// Webhook that need webhooksecret key
        /// </summary>
        /// <param name="webhookSecretKey">Your Webhook Secret.</param>
        public Webhook(string webhookSecretKey)
        {
            mWebhookSecretKey = webhookSecretKey;
        }

        /// <summary>
        /// Verify the requested Signature and Payload
        /// </summary>
        /// <param name="payLoad">request body</param>
        /// <param name="signature">x-request-signature</param>
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
                payloadHashBase64 = System.Convert.ToBase64String(payloadHashedBytes);
            }

            return payloadHashBase64 == signature;
        }
    }
}