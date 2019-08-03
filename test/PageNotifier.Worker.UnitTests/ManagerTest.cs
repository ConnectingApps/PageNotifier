using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using Xunit;

namespace PageNotifier.Worker.UnitTests
{
    public class ManagerTest
    {
        private readonly Mock<IWebsiteReader> _readerMock;
        private readonly Mock<IStorage> _storageMock;
        private readonly Mock<IWebsiteShower> _websiteShowerMock;
        private readonly Manager _manager;

        public ManagerTest()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _readerMock = fixture.Freeze<Mock<IWebsiteReader>>();
            _storageMock = fixture.Freeze<Mock<IStorage>>();
            _websiteShowerMock = fixture.Freeze<Mock<IWebsiteShower>>();
            _manager = fixture.Create<Manager>();
        }

        [Fact]
        public async Task Test()
        {
            _manager.Initialize("someFileName.json");
            

            _storageMock.Setup(a => a.GetUrls()).Returns(new []
            {
                "https://www.example.com","https://www.example.nl"
            });
           
            _storageMock.Setup(s => s.UpdatePageCharacters("https://www.example.com", 
                It.IsAny<int>())).Returns(true);
            _storageMock.Setup(s => s.UpdatePageCharacters("https://www.example.nl",
                It.IsAny<int>())).Returns(false);

            _readerMock.Setup(r => r.ReadPageAsync(It.IsNotNull<string>())).Returns(Task.FromResult("abcd"));

            var actualUpdates = await _manager.NotifyUpdates();

            _storageMock.Verify(s => s.GetUrls(), Times.Once);
            _storageMock.Verify(s => s.Initialize(It.IsNotNull<string>()), Times.Once);
            _storageMock.Verify(s => s.UpdatePageCharacters(It.IsNotNull<string>(), It.IsAny<int>()), Times.Exactly(2));
            _readerMock.Verify(r => r.ReadPageAsync(It.IsNotNull<string>()), Times.Exactly(2));
            _websiteShowerMock.Verify(w => w.ShowWebsite("https://www.example.com"), Times.Once);
            _websiteShowerMock.Verify(w => w.ShowWebsite("https://www.example.nl"), Times.Never);
            Assert.Equal(1, actualUpdates);

            _websiteShowerMock.VerifyNoOtherCalls();
            _readerMock.VerifyNoOtherCalls();
            _storageMock.VerifyNoOtherCalls();
        }
    }
}
