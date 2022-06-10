using Microsoft.Extensions.Configuration;
using RestSharp;

namespace Gluwa.SDK_dotnet.Tests
{
    class Api
    {
        /// <summary>
        /// Execute the process
        /// </summary>
        /// <param name="client">Client object</param>
        /// <param name="request">Request object</param>
        public static IRestResponse GetResponse(RestClient client, RestRequest request)
        {
            return client.Execute(request);
        }


        /// <summary>
        /// Return Response
        /// </summary>
        /// <param name="client"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static IRestResponse GetResponse(RestClient client, IRestRequest request)
        {
            return client.Execute(request);
        }


        /// <summary>
        /// Executes Get request
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static RestRequest SendRequest(Method method)
        {
            return new RestRequest(method);

        }


        /// <summary>
        /// Executes requests with body (PATCH/PUT/POST)
        /// </summary>
        /// <param name="method">HttpMethod</param>
        /// <param name="body">Body of the test</param>
        /// <returns></returns>
        public static RestRequest SendRequest(Method method, object body)
        {
            var restRequest = new RestRequest(method);
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddJsonBody(body);
            return restRequest;
        }


        /// <summary>
        /// Send request without [Body] object
        /// </summary>
        /// <param name="method">HttpMethod</param>
        public static RestRequest SendRequest(Method method, string privateKey)
        {
            var restRequest = new RestRequest(method);
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddHeader("X-REQUEST-SIGNATURE", Shared.GetXRequestSignature(privateKey));
            restRequest.AddQueryParameter("status", "Processed");
            restRequest.AddQueryParameter("limit", "1");
            return restRequest;
        }

        /// <summary>
        /// Full Api URL. Concatenated with const base API URI
        /// </summary>
        /// <param name="endpoint">Api Uri</param>
        public static RestClient SetUrl(string BASE_URI, string endpoint)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddJsonFile("appsettings.dev.json", true).Build();
            var url = BASE_URI + endpoint;
            return new RestClient(url)
            {
                Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(config["API_KEY_TEST"], config["API_SECRET_TEST"])
            };
        }


        /// <summary>
        /// Full Api URL. Concatenated with const base API URI
        /// </summary>
        /// <param name="endpoint">Api Uri</param>
        public static RestClient SetUrlAndClient(string BASE_URI, string endpoint)
        {
            var url = BASE_URI + endpoint;
            return new RestClient(url)
            {
                Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator(Shared.API_KEY, Shared.API_SECRET)
            };
        }
    }
}

