using System.Threading.Tasks;

namespace PageNotifier.Worker
{
    public interface IWebsiteReader
    {
        Task<string> ReadPageAsync(string url);
    }
}