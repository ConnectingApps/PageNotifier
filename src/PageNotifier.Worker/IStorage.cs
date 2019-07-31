using System.Collections.Generic;

namespace PageNotifier.Worker
{
    public interface IStorage
    {
        void Initialize(string filePath);
        bool UpdatePageCharacters(string url, int numberOfAlphanumericCharacters);
        IEnumerable<string> GetUrls();
    }
}