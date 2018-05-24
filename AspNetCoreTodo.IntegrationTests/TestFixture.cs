using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace AspNetCoreTodo.IntegrationTests
{
    public class TestFixture : IDisposable
    {
        private readonly TestServer _server;
        public HttpClient Client { get; set; }

        public TestFixture()
        {
            var builder = new WebHostBuilder().UseStartup<Startup>().ConfigureAppConfiguration(configureDelegate);
            _server = new TestServer(builder);
            Client = _server.CreateClient();
        }

        private void configureDelegate(WebHostBuilderContext context , IConfigurationBuilder configBuilder)
        {
            configBuilder.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\..\\AspNetCoreTodo"));
            configBuilder.AddJsonFile("appsettings.json");
            configBuilder.AddInMemoryCollection(new Dictionary<string, string>()
            {
                ["Facebook:AppId"] = "fake-app-id",
                ["Facebook:AppSecret"] = "fake-app-secret"
            });
        }

        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
        }
    }
}
