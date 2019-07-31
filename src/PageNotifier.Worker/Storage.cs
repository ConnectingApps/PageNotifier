using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PageNotifier.Worker.Models;

namespace PageNotifier.Worker
{
    public class Storage : IStorage
    {
        private string _filePath;

        public void Initialize(string filePath)
        {
            _filePath = filePath;
        }

        public bool UpdatePageCharacters(string url, int numberOfAlphanumericCharacters)
        {
            var fileContent = File.ReadAllText(_filePath);
            var storedData = JsonConvert.DeserializeObject<StorageDescription>(fileContent);
            var webSiteDescription = storedData.WebPageDescriptions.FirstOrDefault(d => d.Url == url);

            if (webSiteDescription == null)
            {
                storedData.WebPageDescriptions.Add(new WebPageDescription
                {
                    Url = url,
                    NumberOfAlphaNumericCharacters = numberOfAlphanumericCharacters
                });

                SaveData(storedData);
                return true;
            }

            if (Math.Abs(webSiteDescription.NumberOfAlphaNumericCharacters - numberOfAlphanumericCharacters) > 15)
            {
                webSiteDescription.NumberOfAlphaNumericCharacters = numberOfAlphanumericCharacters;
                SaveData(storedData);
                return true;
            }

            return false;
        }

        public IEnumerable<string> GetUrls()
        {
            var fileContent = File.ReadAllText(_filePath);
            var storedData = JsonConvert.DeserializeObject<StorageDescription>(fileContent);
            return storedData.WebPageDescriptions.Select(d => d.Url);
        }

        private void SaveData(StorageDescription storage)
        {
            var newFileContent = JsonConvert.SerializeObject(storage);
            File.WriteAllText(_filePath, newFileContent);
        }
    }
}
