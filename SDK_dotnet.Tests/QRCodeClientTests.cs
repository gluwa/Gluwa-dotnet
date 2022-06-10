using Gluwa.SDK_dotnet.Clients;
using Gluwa.SDK_dotnet.Models;
using Gluwa.SDK_dotnet.Tests;
using Newtonsoft.Json;
using NUnit.Framework;
using QRCodeDecoderLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace QRCodeClientTests
{
    [TestFixture("Sandbox")]
    //[TestFixture("Production")]
    public class QRCodeClientTests
    {
        private readonly string useEnvironment;

        /// <summary>
        /// From TestFixture
        /// </summary>
        /// <param name="platform"></param>
        public QRCodeClientTests(string useEnvironment)
        {
            this.useEnvironment = useEnvironment;
        }

        [SetUp]
        public void Setup()
        {
            Shared.SetEnvironmentVariables(useEnvironment);
        }

        [Test]
        public async Task BasicQrCodeWithPayloadTest()
        {
            QRCodeClient qrCodeClient = new QRCodeClient(true);

            // Arange
            string apiKey = Shared.API_KEY;
            string secret = Shared.API_SECRET;
            string address = Shared.SRC_ADR_sUSDCG;
            string privateKey = Shared.SRC_PRIVATE_sUSDCG;
            EPaymentCurrency currency = EPaymentCurrency.sUSDCG;
            decimal amount = 54.893006m;
            string format = null;                            // defaults to null. Returns base64 string. Optional.
                                                             // if you want to receive an image file put ‘image/jpeg’ or ‘image/png’ instead.
            string note = "Note";                            // default to null. Optional
            string merchantOrderID = "Merchant Order ID";    // default to null. Optional
            int expiry = 1800;                               // default to 1800. Optional”

            // Call
            var request = new QRCodeRequest()
            {
                ApiKey = apiKey,
                Secret = secret,
                Address = address,
                PrivateKey = privateKey,
                Currency = currency,
                Amount = amount.ToString(),
                Format = format,
                Note = note,
                MerchantOrderID = merchantOrderID,
                Expiry = expiry
            };
            var result = await qrCodeClient.GetPaymentQRCodePayLoadAsync(request);

            var fileName = Path.GetTempFileName();
            File.WriteAllBytes(fileName, Convert.FromBase64String(result.Data.Base64.ToString()));

            QRDecoder QRDecoder = new QRDecoder();
            Bitmap QRCodeInputImage = new Bitmap(fileName);
            byte[][] DataByteArray = QRDecoder.ImageDecoder(QRCodeInputImage);

            // convert binary result to text string
            string qrResult = QRDecoder.ByteArrayToStr(DataByteArray[0]);
            var qrResultObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(qrResult);

            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            TestContext.WriteLine(JsonConvert.SerializeObject(qrResultObject));

            var qrAmount = Convert.ToDecimal(qrResultObject.Amount);
            var qaFee = Convert.ToDecimal(qrResultObject.Fee);
            var calcalatedTotal = qrAmount + qaFee;

            // Assert
            Assert.AreEqual(amount, calcalatedTotal);
            Assert.AreEqual(note, qrResultObject.Note.ToString());
            Assert.AreEqual(merchantOrderID, qrResultObject.MerchantOrderID.ToString());
            Assert.AreEqual(useEnvironment, qrResultObject.Environment.ToString());
            Assert.IsTrue(result.Data.Data != null);
        }

        [Test]
        public async Task BasicQrCodeTest()
        {
            QRCodeClient qrCodeClient = new QRCodeClient(true);

            // Arange
            string apiKey = Shared.API_KEY;
            string secret = Shared.API_SECRET;
            string address = Shared.SRC_ADR_sUSDCG;
            string privateKey = Shared.SRC_PRIVATE_sUSDCG;
            EPaymentCurrency currency = EPaymentCurrency.sUSDCG;
            decimal amount = 54.893006m;
            string format = null;                            // defaults to null. Returns base64 string. Optional.
                                                             // if you want to receive an image file put ‘image/jpeg’ or ‘image/png’ instead.
            string note = "Note";                            // default to null. Optional
            string merchantOrderID = "Merchant Order ID";    // default to null. Optional
            int expiry = 1800;                               // default to 1800. Optional”

            // Call
            var request = new QRCodeRequest()
            {
                ApiKey = apiKey,
                Secret = secret,
                Address = address,
                PrivateKey = privateKey,
                Currency = currency,
                Amount = amount.ToString(),
                Format = format,
                Note = note,
                MerchantOrderID = merchantOrderID,
                Expiry = expiry
            };
            var result = await qrCodeClient.GetPaymentQRCodeAsync(request);

            var fileName = Path.GetTempFileName();
            File.WriteAllBytes(fileName, Convert.FromBase64String(result.Data));

            QRDecoder QRDecoder = new QRDecoder();
            Bitmap QRCodeInputImage = new Bitmap(fileName);
            byte[][] DataByteArray = QRDecoder.ImageDecoder(QRCodeInputImage);

            // convert binary result to text string
            string qrResult = QRDecoder.ByteArrayToStr(DataByteArray[0]);
            var qrResultObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(qrResult);

            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            TestContext.WriteLine(JsonConvert.SerializeObject(qrResultObject));

            var qrAmount = Convert.ToDecimal(qrResultObject.Amount);
            var qaFee = Convert.ToDecimal(qrResultObject.Fee);
            var calcalatedTotal = qrAmount + qaFee;

            // Assert
            Assert.AreEqual(amount, calcalatedTotal);
            Assert.AreEqual(note, qrResultObject.Note.ToString());
            Assert.AreEqual(merchantOrderID, qrResultObject.MerchantOrderID.ToString());
            Assert.AreEqual(useEnvironment, qrResultObject.Environment.ToString());
        }
    }
}