using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace P_Cloud_API.Models
{
    public partial class ControlModule
    {
        public ControlModule()
        {
            ControlModuleInfos = new HashSet<ControlModuleInfo>();
            InverseSuperiorControlModule = new HashSet<ControlModule>();
            ProcessData = new HashSet<ProcessData>();
        }

        public int Id { get; set; }
        public int? EquipmentModuleId { get; set; }
        public int? SuperiorControlModuleId { get; set; }
        [JsonIgnore]
        public int? ControlModuleInfoId { get; set; }

        public virtual ControlModuleInfo? ControlModuleInfo { get; set; }
        [JsonIgnore]
        public virtual EquipmentModule? EquipmentModule { get; set; }
        [JsonIgnore]
        public virtual ControlModule? SuperiorControlModule { get; set; }
        [JsonIgnore]
        public virtual ICollection<ControlModuleInfo> ControlModuleInfos { get; set; }
        [JsonIgnore]
        public virtual ICollection<ControlModule> InverseSuperiorControlModule { get; set; }
        [JsonIgnore]
        public virtual ICollection<ProcessData> ProcessData { get; set; }
    }
}
