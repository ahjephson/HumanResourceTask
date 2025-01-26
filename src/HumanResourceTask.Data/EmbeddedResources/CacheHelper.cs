using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceTask.Data.EmbeddedResources
{
    internal static class CacheHelper
    {
        internal static ConcurrentDictionary<string, string> Cache = new ConcurrentDictionary<string, string>();
    }
}
