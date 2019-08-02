using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

namespace PageNotifier.Worker.UnitTests
{
    public class StorageTest
    {
        private readonly Storage _storage;
        private readonly string _filePath;

        public StorageTest()
        {
            _storage = new Storage();
            var directory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var initialFilePath = Path.Combine(directory, "storagetest.json");
            _filePath = Path.Combine(directory, $"storagetest{Guid.NewGuid()}.json");
            File.Copy(initialFilePath,_filePath);
            _storage.Initialize(_filePath);
        }

        [Theory]
        [InlineData("https://www.luciehorsch.nl/agenda.php", 10, false)]
        [InlineData("https://www.luciehorsch.nl/agenda.php", 987, true)]
        [InlineData("https://www.bing.com", 10, true)]
        public void UpdatePageCharactersTest(string url, int characterCount, bool expectedUpdate)
        {
            var updated = _storage.UpdatePageCharacters(url, characterCount);
            var jsonContent = File.ReadAllText(_filePath);
            Assert.Contains($"{characterCount}", jsonContent);
            Assert.Equal(expectedUpdate, updated);
        }

        [Fact]
        public void GerUrlsTest()
        {
            var urls = _storage.GetUrls().ToList();
            Assert.True(urls.Any());
            Assert.All(urls, url => Assert.StartsWith("http", url));
        }
    }
}