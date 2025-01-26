using System.Reflection;

namespace HumanResourceTask.Data.EmbeddedResources
{
    public abstract class SqlEmbeddedResource
    {
        private readonly Assembly _assembly;
        private readonly string? _namespace;

        protected SqlEmbeddedResource()
        {
            var type = GetType();
            _assembly = type.Assembly;
            _namespace = type.Namespace;
        }

        protected string GetResourceContent(string resourceName)
        {
            var fullName = $"{_namespace}.{resourceName}.sql";

            return CacheHelper.Cache.GetOrAdd(fullName, ReadManifestStream);
        }

        private string ReadManifestStream(string fullName)
        {
            var resourceStream = _assembly.GetManifestResourceStream(fullName);
            if (resourceStream is null)
            {
                throw new ArgumentException($"No resource stream for {fullName}. Resources need a build action of Embedded Resource");
            }

            using (var reader = new StreamReader(resourceStream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
