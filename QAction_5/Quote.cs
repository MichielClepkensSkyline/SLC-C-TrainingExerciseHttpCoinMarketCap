using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAction_5
{
    public class Quote
    {
        [JsonProperty("USD")]
        public USD USD { get; set; }
    }
}
