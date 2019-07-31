using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PageNotifier.Worker
{
    public class Manager : IManager
    {
        private readonly IStorage _storage;
        private readonly IWebsiteReader _websiteReader;
        private readonly IWebsiteShower _websiteShower;

        public Manager(IWebsiteReader websiteReader, IWebsiteShower websiteShower, IStorage storage)
        {
            _websiteReader = websiteReader;
            _websiteShower = websiteShower;
            _storage = storage;
        }

        public void Initialize(string fileNameOfStorage)
        {
            var currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            _storage.Initialize(Path.Combine(currentDirectory, fileNameOfStorage));
        }

        public async Task<int> NotifyUpdates()
        {
            var urls = _storage.GetUrls();
            var numberOfNotifications = 0;

            foreach (var url in urls)
            {
                var content = await _websiteReader.ReadPageAsync(url);
                var numberOfAlphanumericCharacters = content.Count(char.IsLetterOrDigit);
                if (_storage.UpdatePageCharacters(url, numberOfAlphanumericCharacters))
                {
                    _websiteShower.ShowWebsite(url);
                    numberOfNotifications++;
                }
            }

            return numberOfNotifications;
        }
    }
}