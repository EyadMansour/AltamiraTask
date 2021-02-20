namespace Shared
{
    public class KeyValuePairModel<TKey, TValue>
    {
        public TKey Id { get; set; }
        public TValue Label { get; set; }
    }
    public class KeyValuePairModel<TKey>
    {
        public TKey Id { get; set; }
        public string Label { get; set; }
    }
}
