namespace PKWAT.ScoringPoker.Server.Hubs
{
    public class LiveEstimationObserversInMemoryStore : ILiveEstimationObserversInMemoryStore
    {
        private readonly List<LiveEstimationObserverInfo> _observers = new(); // Can be optimalized

        public void AddObserver(LiveEstimationObserverInfo observerInfo)
        {
            _observers.Add(observerInfo);
        }

        public LiveEstimationObserverInfo GetObserver(string connectionId)
        {
            return _observers.FirstOrDefault(x => x.ConnectionId == connectionId);
        }

        public IEnumerable<LiveEstimationObserverInfo> GetObservers(int scoringTaskId)
        {
            return _observers.Where(x => x.ScoringTaskId == scoringTaskId);
        }

        public void RemoveObserver(string connectionId)
        {
            _observers.RemoveAll(x => x.ConnectionId == connectionId);
        }
    }

    public interface ILiveEstimationObserversInMemoryStore
    {
        void AddObserver(LiveEstimationObserverInfo observerInfo);
        void RemoveObserver(string connectionId);
        IEnumerable<LiveEstimationObserverInfo> GetObservers(int scoringTaskId);
        LiveEstimationObserverInfo GetObserver(string connectionId);
    }

    public record LiveEstimationObserverInfo(string UserName, string ConnectionId, int ScoringTaskId);
    
}
