using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Employee.Api.Tests
{
    public static class ResourceHelper
    {
        public static async Task<JToken> GetJsonResource(string resourceName)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            using (var reader = new StreamReader(stream))
            {
                var str = await reader.ReadToEndAsync().ConfigureAwait(false);
                return JToken.Parse(str);
            }
        }
    }
}
