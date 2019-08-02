using System.Linq;
using System.Net;
using System.Threading;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace PageNotifier.Worker.IntegrationTests
{
    public class WebsiteShowerTest
    {
        [Fact]
        public void TestShowWebsite()
        {
            using (var mockServer = FluentMockServer.Start())
            {
                var url = mockServer.Urls.First();
                mockServer.Given(Request.Create().UsingGet())
                    .RespondWith(Response.Create()
                        .WithBody("SuccessFullRequest")
                        .WithStatusCode(HttpStatusCode.OK));

                var shower = new WebsiteShower();
                shower.ShowWebsite(url);

                // The browser needs a few seconds to to open itself and the web page
                Thread.Sleep(3000);
                Assert.True(mockServer.LogEntries.Any());
                mockServer.Stop();
                mockServer.Dispose();
            }
        }
    }
}