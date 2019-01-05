using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AspNetCoreReproFor6415.Models
{
    public class MyModel
    {
        public MyModel()
        {
            X = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
        }

        [JsonProperty("x")]
        public IDictionary<string, double> X { get; set; }

        [JsonProperty("y")]
        public string Y { get; set; }
    }
}
