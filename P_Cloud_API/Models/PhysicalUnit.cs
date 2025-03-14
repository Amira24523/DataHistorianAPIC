using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace P_Cloud_API.Models
{
    public partial class PhysicalUnit
    {
        public PhysicalUnit()
        {
            ControlModuleInfos = new HashSet<ControlModuleInfo>();
        }

        public int Id { get; set; }
        public string? ShortName { get; set; }
        public string? Fullname { get; set; }
        public string? Description { get; set; }

        [JsonIgnore]
        public virtual ICollection<ControlModuleInfo> ControlModuleInfos { get; set; }
    }
}
