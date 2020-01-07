using Gluwa.Models;
using Gluwa.Utils;
using Newtonsoft.Json;
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
        /// <summary>
        /// The constructor
        /// </summary>
        public Webhook()
        {    
        }

        /// <summary>
        /// Verify the requested Signature and Payload
        /// </summary>
        /// <param name="payLoad">Payload</param>
        /// <param name="signature">The value of X-REQUEST-SIGNATURE</param>
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
                payloadHashBase64 = Convert.ToBase64String(payloadHashedBytes);
            }

            return payloadHashBase64 == signature;
        }
    }
}