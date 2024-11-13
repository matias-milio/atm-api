namespace ATM.Core.Contracts
{
    public interface ICacheService
    {
        T Get<T>(string key) where T : class;
        void Set<T>(string key, T value) where T : class;
    }
}
