using System.Threading.Tasks;
using Gluwa.SDK_dotnet.Clients;
using Gluwa.SDK_dotnet.Models;
using NUnit.Framework;
using SDK_dotnet.Tests.Helpers;
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
                request.ApiKey = ConfigurationHelper.GetByKey("sandboxApiKey");
                request.Secret = ConfigurationHelper.GetByKey("sandboxApiSecret");
            }
            else
            {
                request.ApiKey = ConfigurationHelper.GetByKey("apiKey");
                request.Secret = ConfigurationHelper.GetByKey("apiSecret");
            }
            request.Address = ConfigurationHelper.GetByKey("address");
            request.PrivateKey = ConfigurationHelper.GetByKey("privateKey");
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
                request.ApiKey = ConfigurationHelper.GetByKey("sandboxApiKey");
                request.Secret = ConfigurationHelper.GetByKey("sandboxApiSecret");
            }
            else
            {
                request.ApiKey = ConfigurationHelper.GetByKey("apiKey");
                request.Secret = ConfigurationHelper.GetByKey("apiSecret");
            }
            request.Address = ConfigurationHelper.GetByKey("address");
            request.PrivateKey = ConfigurationHelper.GetByKey("privateKey");
            request.Currency = EPaymentCurrency.sNGNG;
            request.Amount = "1000";

            var response = await qRCodeClient.GetPaymentQRCodePayLoadAsync(request);

            Assert.IsTrue(response.IsSuccess);
            Assert.IsFalse(response.IsFailure);
        }
    }
}