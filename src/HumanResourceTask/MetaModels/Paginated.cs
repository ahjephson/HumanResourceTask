namespace HumanResourceTask.MetaModels
{
    public class Paginated<T>
    {
        public static Paginated<T> Empty = new Paginated<T>([], 0, false);

        public Paginated(IEnumerable<T> items, long total, bool hasMore)
        {
            Items = items;
            Total = total;
            HasMore = hasMore;
        }

        public IEnumerable<T> Items { get; }

        public long Total { get; }

        public bool HasMore { get; }
    }
}
