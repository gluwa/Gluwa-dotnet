using Gluwa.SDK_dotnet.Clients;
using Gluwa.SDK_dotnet.Models;
using Gluwa.SDK_dotnet.Tests;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assert = NUnit.Framework.Assert;
using DescriptionAttribute = NUnit.Framework.DescriptionAttribute;
using TestContext = NUnit.Framework.TestContext;

namespace GluwaClientTests
{
    [TestFixture("Test")]
    [TestFixture("Production")]
    class PostTransactionTests
    {
        private readonly string useEnvironment;

        /// <summary>
        /// From TestFixture
        /// </summary>
        /// <param name="platform"></param>
        public PostTransactionTests(string useEnvironment)
        {
            this.useEnvironment = useEnvironment;
        }

        [SetUp]
        public void Setup()
        {
            if (useEnvironment == "Production" && TestContext.CurrentContext.Test.Properties["Category"].Contains("SandboxOnly"))
            {
                Assert.Inconclusive($"Cannot run this test on environment: {useEnvironment}");
            }
            Shared.SetEnvironmentVariables(useEnvironment);
        }

        [Test]
        [Category("SandboxOnly")]
        [Description("TestCaseId: C1103")]
        public async Task PostTransaction_sUsdcg_Pos()
        {
            // Arrange
            GluwaClient gluwaClient = new GluwaClient(Shared.BASE_ENV);

            // Call
            var request = new CreateTransactionRequest()
            {
                Currency = ECurrency.sUSDCG,
                Address = Shared.SRC_ADR_sUSDCG,
                PrivateKey = Shared.SRC_PRIVATE_sUSDCG,
                Amount = "1",
                Target = Shared.TRG_ADR_sUSDCG
            };
            var result = await gluwaClient.CreateTransactionAsync(request);

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.IsTrue(result.IsSuccess);
        }


        [Test]
        [Category("SandboxOnly")]
        [Description("TestCaseId: C1103")]
        public async Task PostTransaction_sNgNg_Pos()
        {
            // Arrange
            GluwaClient gluwaClient = new GluwaClient(Shared.BASE_ENV);

            // Call
            var request = new CreateTransactionRequest()
            {
                Currency = ECurrency.sNGNG,
                Address = Shared.SRC_ADR_sNGNG,
                PrivateKey = Shared.SRC_PRIVATE_sNGNG,
                Amount = "10",
                Target = Shared.TRG_ADR_sNGNG,
                Nonce = Shared.GetNonceString(75)
            };
            var result = await gluwaClient.CreateTransactionAsync(request);

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.AreEqual(null, result.Error);
        }


        [Test]
        [Category("SandboxOnly")]
        [Description("TestCaseId: C1103")]
        public async Task PostTransaction_NgNg_Pos()
        {
            // Arrange
            GluwaClient gluwaClient = new GluwaClient(Shared.BASE_ENV);

            // Call
            var request = new CreateTransactionRequest()
            {
                Currency = ECurrency.NGNG,
                Address = Shared.SRC_ADR_NGNG,
                PrivateKey = Shared.SRC_PRIVATE_NGNG,
                Amount = "1",
                Target = Shared.TRG_ADR_NGNG
            };

            var result = await gluwaClient.CreateTransactionAsync(request);

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.IsTrue(result.IsSuccess);
        }


        [Test]
        [Category("SandboxOnly")]
        [Description("TestCaseId: C1103")]
        public async Task PostTransaction_Btc_Pos()
        {
            // Arrange
            GluwaClient gluwaClient = new GluwaClient(Shared.BASE_ENV);

            // Call
            var request = new CreateTransactionRequest()
            {
                Currency = ECurrency.BTC,
                Address = Shared.SRC_ADR_BTC,
                PrivateKey = Shared.SRC_PRIVATE_BTC,
                Amount = "0.00001",
                Target = Shared.TRG_ADR_BTC
            };
            var result = await gluwaClient.CreateTransactionAsync(request);

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.IsTrue(result.IsSuccess);
        }


        [Test]
        [Description("TestCaseId: C1104")]
        public async Task PostTransaction_SameSendReceiveAddress_Neg()
        {
            // Arrange
            GluwaClient gluwaClient = new GluwaClient(Shared.BASE_ENV);

            // Call
            var request = new CreateTransactionRequest()
            {
                Currency = ECurrency.sNGNG,
                Address = Shared.SRC_ADR_sNGNG,
                PrivateKey = Shared.SRC_PRIVATE_sNGNG,
                Amount = "1",
                Target = Shared.SRC_ADR_sNGNG
            };

            var result = await gluwaClient.CreateTransactionAsync(request);

            // Assert
            TestContext.WriteLine($"Testing against: {useEnvironment}");
            TestContext.WriteLine(JsonConvert.SerializeObject(result));
            Assert.AreEqual("ValidationError", result.Error.Code);
            Assert.AreEqual("Source and Target addresses are the same.", result.Error.InnerErrors[0].Message);
        }


        //TODO: Revise after fix. Expected - Code":"ValidationError" with error Msg.; Actual - Error:System.FormatException
        //[Test]
        //public async Task PostTransaction_InvalidSourceAddress_Neg()
        //{
        //    // Arrange
        //    GluwaClient gluwaClient = new GluwaClient(Shared.BASE_ENV);

        //    // Call
        //    var result = await gluwaClient.CreateTransactionAsync(
        //        ECurrency.sNGNG,
        //        Shared.INVALID_ETH_ADDRESS,    // invalid source address
        //        Shared.SRC_PRIVATE_sNGNG,
        //        "1",
        //        Shared.TRG_ADR_sNGNG
        //    );

        //    // Assert
        //    TestContext.WriteLine($"Testing against: {useEnvironment}");
        //    TestContext.WriteLine(JsonConvert.SerializeObject(result));
        //    Assert.AreEqual("", result.Error.Code);
        //    Assert.AreEqual("", result.Error.InnerErrors[0].Message);
        //}

        ////TODO: Revise after fix. Expected - Code":"ValidationError" with error Msg.; Actual - Error:System.FormatException
        //[Test]
        //[Description("TestCaseId: C1665")]
        //public async Task PostTransaction_InvalidPrivateKey_Neg()
        //{
        //    // Arrange
        //    GluwaClient gluwaClient = new GluwaClient(Shared.BASE_ENV);

        //    // Call
        //    var result = await gluwaClient.CreateTransactionAsync(
        //        ECurrency.sNGNG,            
        //        Shared.SRC_ADR_sNGNG,       
        //        Shared.INVALID_PRIVATE_KEY,   // invalid private key
        //        "10",                       
        //        Shared.TRG_ADR_sNGNG,       
        //        null,
        //        null,
        //        Shared.GetNonceString(75)   
        //    );

        //    // Assert 
        //    TestContext.WriteLine($"Testing against: {useEnvironment}");
        //    TestContext.WriteLine(JsonConvert.SerializeObject(result));
        //    Assert.IsTrue(result.IsFailure);
        //    Assert.AreEqual("BadRequest", result.Error.Code);
        //    Assert.AreEqual("Signature is invalid", result.Error.Message);
        //}


        ////TODO: Revise after fix. Expected - Code":"ValidationError" with error Msg.; Actual - Error:System.FormatException
        //[Test]
        //[Description("TestCaseId: C1664")]
        //public async Task PostTransaction_InvalidTargetAddress_Neg()
        //{
        //    // Arrange
        //    GluwaClient gluwaClient = new GluwaClient(Shared.BASE_ENV);

        //    // Call
        //    var result = await gluwaClient.CreateTransactionAsync(
        //        ECurrency.sUSDCG,
        //        Shared.SRC_ADR_sUSDCG,
        //        Shared.SRC_PRIVATE_sUSDCG,
        //        "1",
        //        Shared.INVALID_ETH_ADDRESS       // invalid target address
        //    );

        //    // Assert
        //    TestContext.WriteLine($"Testing against: {useEnvironment}");
        //    TestContext.WriteLine(JsonConvert.SerializeObject(result));
        //    Assert.AreEqual("", result.Error.Code);
        //    Assert.AreEqual("", result.Error.InnerErrors[0].Message);
        //}


        ////TODO: Revise after fix. Expected - Code":"ValidationError" with error Msg.; Actual - Error:System.FormatException
        //[Test]
        //[Description("TestCaseId: C1852")]
        //public async Task PostTransaction_EmptyAddress_Pos()
        //{
        //    // Arrange
        //    GluwaClient gluwaClient = new GluwaClient(Shared.BASE_ENV);

        //    // Call
        //    var result = await gluwaClient.CreateTransactionAsync(
        //        ECurrency.sUSDCG,
        //        null,                       // missing address
        //        Shared.SRC_PRIVATE_sUSDCG,
        //        "1",
        //        Shared.TRG_ADR_sUSDCG
        //    );

        //    // Assert
        //    TestContext.WriteLine($"Testing against: {useEnvironment}");
        //    TestContext.WriteLine(JsonConvert.SerializeObject(result));
        //    Assert.IsTrue(result.IsSuccess);
        //}


        ////TODO: Revise after fix. Expected - Code":"ValidationError" with error Msg.; Actual - Error:System.FormatException
        //[Test]
        //[Description("TestCaseId: C1853")]
        //public async Task PostTransaction_EmptyPrivateKey_Pos()
        //{
        //    // Arrange
        //    GluwaClient gluwaClient = new GluwaClient(Shared.BASE_ENV);

        //    // Call
        //    var result = await gluwaClient.CreateTransactionAsync(
        //        ECurrency.sUSDCG,
        //        Shared.SRC_ADR_sUSDCG,
        //        null,                       // missing private key
        //        "1",
        //        Shared.TRG_ADR_sUSDCG
        //    );

        //    // Assert
        //    TestContext.WriteLine($"Testing against: {useEnvironment}");
        //    TestContext.WriteLine(JsonConvert.SerializeObject(result));
        //    Assert.IsTrue(result.IsSuccess);
        //}
    }
}
