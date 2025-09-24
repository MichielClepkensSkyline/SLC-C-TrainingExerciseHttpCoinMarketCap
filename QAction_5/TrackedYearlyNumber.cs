using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAction_5
{
    public class TrackedYearlyNumber
    {
        [JsonProperty("maxIncrementalNumber")]
        public int MaxIncrementalNumber { get; set; }

        [JsonProperty("minIncrementalNumber")]
        public int MinIncrementalNumber { get; set; }

        [JsonProperty("maxIncrementalDate")]
        public DateTime MaxIncrementalDate { get; set; }

        [JsonProperty("minIncrementalDate")]
        public DateTime MinIncrementalDate { get; set; }
    }
}
