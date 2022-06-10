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
    class GetTransactionsTests
    {
        private readonly string useEnvironment;

        /// <summary>
        /// From TestFixture
        /// </summary>
        /// <param name="platform"></param>
        public GetTransactionsTests(string useEnvironment)
        {
            this.useEnvironment = useEnvironment;
        }

        [SetUp]
        public void Setup()
        {
            Shared.SetEnvironmentVariables(useEnvironment);
        }

        [Test]
        [Description("TestCaseId: C1105")]
        public async Task GetTransactions_sUsdcg_Pos()
        {
            // Arrange
            GluwaClient gluwaClient = new GluwaClient(Shared.BASE_ENV);

            // Call
            var result = await gluwaClient.GetTransactionListAsync(
                ECurrency.sUSDCG,                     // requied currency
                Shared.SRC_ADR_sUSDCG,                // requied source address
                Shared.SRC_PRIVATE_sUSDCG,            // requied private key
                100,                                  // optional limit, default = 100
                ETransactionStatusFilter.Confirmed,   // optional status, default = Confirmed
                10                                    // optional offset, default = 0
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.AreEqual(ECurrency.sUSDCG, result.Data[0].Currency);
            Assert.Contains(Shared.SRC_ADR_sUSDCG.ToLower(), new[] { result.Data[0].Sources[0].ToLower(), result.Data[0].Targets[0].ToLower() },
            $"Expected {Shared.SRC_ADR_sUSDCG} but found source {result.Data[0].Sources[0]} and target {result.Data[0].Targets[0]}");
        }


        [Test]
        [Description("TestCaseId: C1105")]
        public async Task GetTransactions_Btc_Pos()
        {
            // Arrange
            GluwaClient gluwaClient = new GluwaClient(Shared.BASE_ENV);

            // Call
            var result = await gluwaClient.GetTransactionListAsync(
                ECurrency.BTC,                    
                Shared.SRC_ADR_BTC,                
                Shared.SRC_PRIVATE_BTC                                   
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.AreEqual(ECurrency.BTC, result.Data[0].Currency);
            Assert.Contains(Shared.SRC_ADR_BTC.ToLower(), new[] { result.Data[0].Sources[0].ToLower(), result.Data[0].Targets[0].ToLower() },
            $"Expected {Shared.SRC_ADR_BTC} but found source {result.Data[0].Sources[0]} and target {result.Data[0].Targets[0]}");
        }


        [Test]
        [Description("TestCaseId: C1105")]
        public async Task GetTransactions_NgNg_Pos()
        {
            // Arrange
            GluwaClient gluwaClient = new GluwaClient(Shared.BASE_ENV);

            // Call
            var result = await gluwaClient.GetTransactionListAsync(
                ECurrency.NGNG,             
                Shared.SRC_ADR_NGNG,        
                Shared.SRC_PRIVATE_NGNG  
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.AreEqual(ECurrency.NGNG, result.Data[0].Currency);
            // SRC_ADR_sNGNG can be either source or target
            Assert.Contains(Shared.SRC_ADR_NGNG.ToLower(), new[] { result.Data[0].Sources[0].ToLower(), result.Data[0].Targets[0].ToLower() },
            $"Expected {Shared.SRC_ADR_NGNG} but found source {result.Data[0].Sources[0]} and target {result.Data[0].Targets[0]}");
        }


        [Test]
        [Description("TestCaseId: C1105")]
        public async Task GetTransactions_sNgNg_Pos()
        {
            // Arrange
            GluwaClient gluwaClient = new GluwaClient(Shared.BASE_ENV);

            // Call
            var result = await gluwaClient.GetTransactionListAsync(
                ECurrency.sNGNG,
                Shared.SRC_ADR_sNGNG,
                Shared.SRC_PRIVATE_sNGNG
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.AreEqual(ECurrency.sNGNG, result.Data[0].Currency);
            // SRC_ADR_sNGNG can be either source or target
            Assert.Contains(Shared.SRC_ADR_sNGNG.ToLower(), new[] { result.Data[0].Sources[0].ToLower(), result.Data[0].Targets[0].ToLower() },
            $"Expected {Shared.SRC_ADR_sNGNG} but found source {result.Data[0].Sources[0]} and target {result.Data[0].Targets[0]}");
        }


        [Test]
        [Description("TestCaseId: C1667")]
        public async Task GetTransactions_InvalidAddress_Neg()
        {
            // Arrange
            GluwaClient gluwaClient = new GluwaClient(Shared.BASE_ENV);

            // Call
            var result = await gluwaClient.GetTransactionListAsync(
                ECurrency.sUSDCG,
                Shared.INVALID_ETH_ADDRESS,       // invalid source address
                Shared.SRC_PRIVATE_sUSDCG
            );

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual("InvalidSignature", result.Error.Code);
            Assert.AreEqual("The supplied signature is not authorized for this resource.", result.Error.Message);
        }


        [Test]
        public async Task GetTransactions_InvalidKey_Neg()
        {
            // Arrange
            GluwaClient gluwaClient = new GluwaClient(Shared.BASE_ENV);

            // Call
            var result = await gluwaClient.GetTransactionListAsync(
                ECurrency.sUSDCG,
                Shared.SRC_ADR_sUSDCG,
                Shared.INVALID_PRIVATE_KEY        // invalid private key
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
