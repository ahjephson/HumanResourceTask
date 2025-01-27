using System.Collections.Concurrent;

namespace HumanResourceTask.Data.EmbeddedResources
{
    internal static class CacheHelper
    {
        internal static ConcurrentDictionary<string, string> Cache = new ConcurrentDictionary<string, string>();
    }
}
