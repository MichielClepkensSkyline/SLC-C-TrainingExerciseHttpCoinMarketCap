using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System;
using QAction_1;

namespace QAction_5
{
    public class LatestQuotes
    {
        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }
}
