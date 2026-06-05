using System.Collections.Generic;

namespace partycli.Models
{
    public class CommandSettings
    {
        public string Description { get; set; }
        public List<ParamEntry> Params { get; set; } = new List<ParamEntry>();
    }
}
