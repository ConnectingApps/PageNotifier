using System.Threading.Tasks;

namespace PageNotifier.Worker
{
    public interface IManager
    {
        void Initialize(string fileNameOfStorage);
        Task<int> NotifyUpdates();
    }
}