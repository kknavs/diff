using System.Text;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using System.Text.Json;
using System.Net.Mime;

namespace DiffApplication.Test.Functional
{
    public class TestBase
    {
        protected IConfigurationRoot Configuration { get; private set; }

        protected ILogger loggerTest;
        protected ILoggerFactory loggerFactory;
        protected TestServer server;
        protected HttpClient client;

        public TestBase() 
        {
            string appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

            Configuration = new ConfigurationBuilder()
              .AddJsonFile(Path.Combine(appPath, "appsettings.json"))
              .AddJsonFile(Path.Combine(appPath, "appsettings.test.json"), optional: true)
              .AddJsonFile(Path.Combine(appPath, "appsettings.test.functional.development.json"), optional: true)
              .AddEnvironmentVariables()
              .Build();

            server = CreateServer(appPath);

            client = server.CreateClient();

            loggerFactory = server.Services.GetRequiredService<ILoggerFactory>();
            loggerTest = loggerFactory.CreateLogger(Assembly.GetExecutingAssembly().FullName!);

            loggerTest.LogInformation("Path: {path}", appPath);
            //AppSettings = Configuration.GetSection("AppSettings").Get<TAppSettings>();
        }

        private TestServer CreateServer(string appPath)
        {
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.UseContentRoot(appPath);
                    builder.ConfigureTestServices(services =>
                    {
                        // configure
                    });
                    builder.ConfigureAppConfiguration(cb =>
                    {
                        cb.AddConfiguration(Configuration);
                    });
                    builder.ConfigureLogging((context, logging) =>
                    {
                        logging.AddConfiguration(context.Configuration.GetSection("logging"));
                    });
                    builder.UseEnvironment("Test");
                }
             );

            return application.Server;
        }

        private static HttpRequestMessage BuildHttpRequestMessage(HttpMethod httpMethod, string url, HttpContent? content = null)
        {
            var requestMessage = new HttpRequestMessage(httpMethod, url);
            if (content != null)
            {
                requestMessage.Content = content;
            }

            return requestMessage;
        }

        protected async Task<HttpResponseMessage> PerformRequestAsync(HttpClient client, HttpMethod httpMethod, string uri, HttpContent? content = null)
        {
            var reqMessage = BuildHttpRequestMessage(httpMethod, uri, content);
            return await client.SendAsync(reqMessage);
        }

        protected async Task<TResponse?> GetAssertAsync<TResponse>(
          HttpClient client,
          string uri,
          HttpStatusCode expectedStatusCode)
          where TResponse : class
        {
            var httpResponse = await PerformRequestAsync(client, HttpMethod.Get, uri);

            Assert.AreEqual(expectedStatusCode, httpResponse.StatusCode);

            string responseString = await httpResponse.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseString))
            {
                return default;
            }

            if (httpResponse.IsSuccessStatusCode) // Only try to deserialize in case there are no exception
            {
                return JsonSerializer.Deserialize<TResponse>(responseString);
            }
            return null;
        }

        protected async Task<HttpResponseMessage> PutAssertAsync<TRequest>(
            HttpClient client,
            string url,
            TRequest request,
            HttpStatusCode expectedStatusCode)
        {
            var httpResponse = await PerformRequestAsync(client, HttpMethod.Put, url, new StringContent(JsonSerializer.Serialize(request),
                Encoding.UTF8, MediaTypeNames.Application.Json));

            if (expectedStatusCode != httpResponse.StatusCode)
            {
                // include body in assert message to make debugging easier
                if (expectedStatusCode != httpResponse.StatusCode)
                {
                    var body = await httpResponse.Content.ReadAsStringAsync();
                    loggerTest.LogInformation("CheckAndAssertFailedResponse body: {body}", body);
                }
                Assert.AreEqual(expectedStatusCode, httpResponse.StatusCode);
            }

            return httpResponse;
        }

        protected async Task<TResponse?> ParseResponse<TResponse>(HttpResponseMessage httpResponse) where TResponse : class
        {
            if (httpResponse.IsSuccessStatusCode) // Only try to deserialize in case there are no exception
            {
                string responseString = await httpResponse.Content.ReadAsStringAsync();
                TResponse response = JsonSerializer.Deserialize<TResponse>(responseString)!;
                return response;
            }

            return null;
        }
    }
}
