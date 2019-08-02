using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace PageNotifier.Worker.IntegrationTests
{
    public class WebsiteReaderTest : IDisposable
    {
        private readonly FluentMockServer _mockServer;

        public WebsiteReaderTest()
        {
            _mockServer = FluentMockServer.Start();
        }

        [Fact]
        public async Task TestReadPage()
        {

            var url = _mockServer.Urls.First();
            _mockServer.Given(Request.Create().UsingGet())
                       .RespondWith(Response.Create()
                                                    .WithBody("SuccessFullRequest")
                                                    .WithStatusCode(HttpStatusCode.OK));

            using (var client = new HttpClient())
            {
                var reader = new WebsiteReader(client);
                var content = await reader.ReadPageAsync(url);
                Assert.Equal("SuccessFullRequest", content);
            }
        }

        [Theory]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.BadRequest)]
        public async Task TestReadPageFail(HttpStatusCode statusCode)
        {

            var url = _mockServer.Urls.First();
            _mockServer.Given(Request.Create().UsingGet())
                .RespondWith(Response.Create()
                    .WithBody("SuccessFullRequest")
                    .WithStatusCode(statusCode));

            using (var client = new HttpClient())
            {
                var reader = new WebsiteReader(client);
                await Assert.ThrowsAnyAsync<Exception>(() => reader.ReadPageAsync(url));
            }
        }

        public void Dispose()
        {
            _mockServer.Stop();
            _mockServer.Dispose();
        }
    }
}
