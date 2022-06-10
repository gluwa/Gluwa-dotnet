using Gluwa.SDK_dotnet.Clients;
using Gluwa.SDK_dotnet.Models;
using Gluwa.SDK_dotnet.Tests;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Threading.Tasks;
using Assert = NUnit.Framework.Assert;
using DescriptionAttribute = NUnit.Framework.DescriptionAttribute;
using TestContext = NUnit.Framework.TestContext;

namespace GluwaClientTests
{
    [TestFixture("Sandbox")]
    [TestFixture("Production")]
    class GetTransactionsDetailsTests
    {
        private readonly string useEnvironment;

        /// <summary>
        /// From TestFixture
        /// </summary>
        /// <param name="platform"></param>
        public GetTransactionsDetailsTests(string useEnvironment)
        {
            this.useEnvironment = useEnvironment;
        }

        [SetUp]
        public void Setup()
        {
            Shared.SetEnvironmentVariables(useEnvironment);
        }

        [Test]
        [Description("TestCaseId: C1106")]
        public async Task GetTransactionDetails_sUsdcg_Pos()
        {
            // Arrange
            GluwaClient gluwaClient = new GluwaClient(Shared.BASE_ENV);

            // Call
            var result = await gluwaClient.GetTransactionDetailsAsync(
            ECurrency.sUSDCG,               // requied currency:sUSDCG
            Shared.SRC_PRIVATE_sUSDCG,      // requied private key
            Shared.TxID_sUSDCG              // requied transaction Id
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.AreEqual(Shared.TxID_sUSDCG, result.Data.TxnHash);
            Assert.AreEqual(Shared.SRC_ADR_sUSDCG.ToLower(), result.Data.Sources[0]);
            Assert.AreEqual(ECurrency.sUSDCG, result.Data.Currency);
        }


        [Test]
        [Description("TestCaseId: C1106")]
        public async Task GetTransactionDetails_sNgNg_Pos()
        {
            // Arrange
            GluwaClient gluwaClient = new GluwaClient(Shared.BASE_ENV);

            // Call
            var result = await gluwaClient.GetTransactionDetailsAsync(
            ECurrency.sNGNG,               // requied currency:sNGNG
            Shared.SRC_PRIVATE_sNGNG,      // requied private key
            Shared.TxID_sNGNG              // requied transaction Id
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.AreEqual(Shared.TxID_sNGNG, result.Data.TxnHash);
            Assert.AreEqual(Shared.SRC_ADR_sNGNG.ToLower(), result.Data.Sources[0]);
            Assert.AreEqual(ECurrency.sNGNG, result.Data.Currency);
        }


        [Test]
        [Description("TestCaseId: C1670")]
        public async Task GetTransactionDetails_InvalidTxId_Neg()
        {
            // Arrange
            GluwaClient gluwaClient = new GluwaClient(Shared.BASE_ENV);

            // Call
            var result = await gluwaClient.GetTransactionDetailsAsync(
            ECurrency.sUSDCG,
            Shared.SRC_PRIVATE_sUSDCG,
            Shared.INVALIDTXNID                    // invalid TxId
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual("NotFound", result.Error.Code);
            Assert.AreEqual("Transaction not found.", result.Error.Message);
        }


        [Test]
        [Description("TestCaseId: C1669")]
        public async Task GetTransactionDetails_InvalidKey_Neg()
        {
            // Arrange
            GluwaClient gluwaClient = new GluwaClient(Shared.BASE_ENV);

            // Call
            var result = await gluwaClient.GetTransactionDetailsAsync(
            ECurrency.sUSDCG,
            Shared.INVALID_PRIVATE_KEY,        // invalid private key
            Shared.TxID_sUSDCG
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual("InvalidSignature", result.Error.Code);
            Assert.AreEqual("The supplied signature is not authorized for this resource.", result.Error.Message);
        }
    }
}
