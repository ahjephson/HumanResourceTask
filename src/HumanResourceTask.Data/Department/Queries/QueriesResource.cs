using HumanResourceTask.Data.EmbeddedResources;

namespace HumanResourceTask.Data.Department.Queries
{
    public class QueriesResource : SqlEmbeddedResource
    {
        private static QueriesResource Instance { get; }

        static QueriesResource()
        {
            Instance = new QueriesResource();
        }

        public static string List => Instance.GetResourceContent(nameof(List));
    }
}
