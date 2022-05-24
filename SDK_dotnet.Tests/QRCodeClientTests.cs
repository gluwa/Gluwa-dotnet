using System.Threading.Tasks;
using Gluwa.SDK_dotnet.Clients;
using Gluwa.SDK_dotnet.Models;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace SDK_dotnet.Tests
{
    [TestFixture]
    public class QRCodeClientTests
    {
        [TestCase(true)]
        [TestCase(false)]
        public async Task GetQRCodeTest(bool bSandbox)
        {
            QRCodeClient qRCodeClient = new QRCodeClient(bSandbox);

            QRCodeRequest request = new QRCodeRequest();
            
            if (bSandbox)
            {
                request.ApiKey = "sandbox apiKey";
                request.Secret = "sandbox secret";
            }
            else
            {
                request.ApiKey = "apiKey";
                request.Secret = "secret";
            }
            request.Address = "address";
            request.PrivateKey = "privateKey";
            request.Currency = EPaymentCurrency.sNGNG;
            request.Amount = "1000";

            var response = await qRCodeClient.GetPaymentQRCodeAsync(request);

            Assert.IsTrue(response.IsSuccess);
            Assert.IsFalse(response.IsFailure);
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task GetQRCodePayLoadTest(bool bSandbox)
        {
            QRCodeClient qRCodeClient = new QRCodeClient(bSandbox);

            QRCodeRequest request = new QRCodeRequest();

            if (bSandbox)
            {
                request.ApiKey = "sandbox apiKey";
                request.Secret = "sandbox secret";
            }
            else
            {
                request.ApiKey = "apiKey";
                request.Secret = "secret";
            }
            request.Address = "address";
            request.PrivateKey = "privateKey";
            request.Currency = EPaymentCurrency.sNGNG;
            request.Amount = "1000";

            var response = await qRCodeClient.GetPaymentQRCodePayLoadAsync(request);

            Assert.IsTrue(response.IsSuccess);
            Assert.IsFalse(response.IsFailure);
        }
    }
}