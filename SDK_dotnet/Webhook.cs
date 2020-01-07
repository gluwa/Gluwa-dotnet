using Gluwa.Models;
using Gluwa.Utils;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Gluwa
{
    /// <summary>
    /// When user completes transfer, the Gluwa API sends a webhook to your webhook endpoint.
    /// Verify that the values ​​actually sent by the Gluwa server are correct.
    /// </summary>
    public sealed class Webhook
    {
        private readonly string mWebhookSecretKey;

        /// <summary>
        /// Constructor
        /// </summary>
        public Webhook()
        {       
        }

        /// <summary>
        /// Verify the requested Signature and Payload
        /// </summary>
        /// <param name="payLoad">request body</param>
        /// <param name="signature">x-request-signature</param>
        /// <param name="webhookSecretKey">Your Webhook Secret.</param>
        public bool ValidateWebhook(PayLoad payLoad, string signature, string webhookSecretKey)
        {
            Converter.Settings.NullValueHandling = NullValueHandling.Include;
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
            byte[] key = Encoding.UTF8.GetBytes(webhookSecretKey);
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