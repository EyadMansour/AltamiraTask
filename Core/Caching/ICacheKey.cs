namespace Core.Caching
{
    public interface ICacheKey<TItem>
    {
        string CacheKey { get; }
    }
}