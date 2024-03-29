﻿using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Employee.Api.Tests
{
    public static class ResourceHelper
    {
        public static async Task<JToken> GetJsonResource(string resourceName)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            using (var reader = new StreamReader(stream))
            {
                var str = await reader.ReadToEndAsync();
                return JToken.Parse(str);
            }
        }
    }
}
