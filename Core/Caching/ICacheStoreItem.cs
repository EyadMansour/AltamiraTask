namespace Core.Caching
{
    public interface ICacheStoreItem
    {
        string CacheKey { get; }
    }
}