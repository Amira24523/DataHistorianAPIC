using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace P_Cloud_API.Models
{
    public partial class EditType
    {
        public EditType()
        {
            ControlModuleInfos = new HashSet<ControlModuleInfo>();
            ProcessData = new HashSet<ProcessData>();
        }

        public int Id { get; set; }
        public string? Description { get; set; }

        [JsonIgnore]
        public virtual ICollection<ControlModuleInfo> ControlModuleInfos { get; set; }
        [JsonIgnore]
        public virtual ICollection<ProcessData> ProcessData { get; set; }
    }
}
