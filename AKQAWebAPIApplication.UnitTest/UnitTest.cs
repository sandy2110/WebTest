using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Web.Http;
using AKQAWebApplicationTest.Controllers;
using AKQAWebApplicationTest.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Threading;
using System.Net.Http.Formatting;

namespace AKQAWebApplicationTest.Tests
{
    [TestClass]
    public class UnitTest
    {
        private HttpServer _server;
        private const string _url = "http://localhost:54106";

        #region Constructor
        public UnitTest()
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(name: "Default", routeTemplate: "api/{controller}/{action}/{id}", defaults: new { id = RouteParameter.Optional });
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            config.MessageHandlers.Add(new WebApiKeyHandler());

            _server = new HttpServer(config);
        }
        #endregion

        #region TestMethods
        /// <summary>
        /// Test Method to check whether webAPI response is Null.
        /// </summary>
        [TestMethod]
        public void IsResponseNotNull()
        {
            var client = new HttpClient(_server);
            var input = new InputForm { Name = "sandeep bhatia", Amount = Convert.ToDecimal(123.45) };
            var request = createRequest("/api/demoapi/show?apikey=test", "application/json", HttpMethod.Post, input, new JsonMediaTypeFormatter());
            var expectedJson = JsonConvert.SerializeObject(input);

            using (HttpResponseMessage response = client.SendAsync(request, new CancellationTokenSource().Token).Result)
            {
                Xunit.Assert.NotNull(response.Content);
            }

            request.Dispose();
        }

        /// <summary>
        /// Test Method to check whether webAPI response is of JSON type.
        /// </summary>
        [TestMethod]
        public void IsJSONResponse()
        {
            var client = new HttpClient(_server);
            var input = new InputForm { Name = "sandeep bhatia", Amount = Convert.ToDecimal(123.45) };
            var request = createRequest("/api/demoapi/show?apikey=test", "application/json", HttpMethod.Post, input, new JsonMediaTypeFormatter());
            var expectedJson = JsonConvert.SerializeObject(input);

            using (HttpResponseMessage response = client.SendAsync(request, new CancellationTokenSource().Token).Result)
            {
                Xunit.Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
            }

            request.Dispose();
        }

        /// <summary>
        /// Test Method to check whether webAPI response is for the given request body matching or not.
        /// </summary>
        [TestMethod]
        public void IsInvalidResponse()
        {
            var client = new HttpClient(_server);
            var input = new InputForm { Name = "sandeep bhatia", Amount = Convert.ToDecimal(123.45) };
            var request = createRequest("/api/demoapi/show?apikey=test", "application/json", HttpMethod.Post, input, new JsonMediaTypeFormatter());
            var expectedJson = JsonConvert.SerializeObject(input);

            using (HttpResponseMessage response = client.SendAsync(request, new CancellationTokenSource().Token).Result)
            {
                Xunit.Assert.NotEqual(expectedJson, response.Content.ReadAsStringAsync().Result);
            }

            request.Dispose();
        }

        /// <summary>
        /// Test Method to check whether webAPI response is for the given request body matching or not.
        /// </summary>
        [TestMethod]
        public void IsResponseMatchingOrNot()
        {
            var client = new HttpClient(_server);
            var input = new InputForm { Name = "sandeep bhatia", Amount = Convert.ToDecimal(123.45) };
            var request = createRequest("/api/demoapi/show?apikey=test", "application/json", HttpMethod.Post, input, new JsonMediaTypeFormatter());
            var expectedJson = JsonConvert.SerializeObject(input);

            using (HttpResponseMessage response = client.SendAsync(request, new CancellationTokenSource().Token).Result)
            {
                Xunit.Assert.DoesNotMatch(expectedJson, response.Content.ReadAsStringAsync().Result);
            }

            request.Dispose();
        }
        #endregion

        #region Private Utility Methods
        private HttpRequestMessage createRequest(string url, string mthv, HttpMethod method)
        {
            var request = new HttpRequestMessage();

            request.RequestUri = new Uri(_url + url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mthv));
            request.Method = method;

            return request;
        }

        private HttpRequestMessage createRequest<T>(string url, string mthv, HttpMethod method, T content, MediaTypeFormatter formatter) where T : class
        {
            HttpRequestMessage request = createRequest(url, mthv, method);
            request.Content = new ObjectContent<T>(content, formatter);

            return request;
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            if (_server != null)
            {
                _server.Dispose();
            }
        }
        #endregion
    }
}
