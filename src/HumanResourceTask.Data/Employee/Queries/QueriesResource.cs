using HumanResourceTask.Data.EmbeddedResources;

namespace HumanResourceTask.Data.Employee.Queries
{
    public class QueriesResource : SqlEmbeddedResource
    {
        private static QueriesResource Instance { get; }

        static QueriesResource()
        {
            Instance = new QueriesResource();
        }

        public static string Count => Instance.GetResourceContent(nameof(Count));

        public static string FindById => Instance.GetResourceContent(nameof(FindById));

        public static string Insert => Instance.GetResourceContent(nameof(Insert));

        public static string List => Instance.GetResourceContent(nameof(List));

        public static string Update => Instance.GetResourceContent(nameof(Update));
    }
}
