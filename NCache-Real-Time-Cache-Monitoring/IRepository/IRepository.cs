namespace NCache_Real_Time_Cache_Monitoring.IRepository
{
    public interface IRepository<T>
    {
        // Cache Operations
        void AddToCache(T item);
        T GetFromCache(int id);
        void InvalidateCache(int id);

        // Database Operations
        void Add(T item);
        T Get(int id);
        void Delete(int id);
    }
}
