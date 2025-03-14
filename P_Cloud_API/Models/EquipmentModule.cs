using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace P_Cloud_API.Models
{
    public partial class EquipmentModule
    {
        public EquipmentModule()
        {
            ControlModules = new HashSet<ControlModule>();
            InverseSuperiorEquipmentModule = new HashSet<EquipmentModule>();
        }

        public int Id { get; set; }
        public int? UnitId { get; set; }
        public int? SuperiorEquipmentModuleId { get; set; }
        public string? Name { get; set; }

        public virtual EquipmentModule? SuperiorEquipmentModule { get; set; }
        public virtual Unit? Unit { get; set; }
        [JsonIgnore]
        public virtual ICollection<ControlModule> ControlModules { get; set; }
        [JsonIgnore]
        public virtual ICollection<EquipmentModule> InverseSuperiorEquipmentModule { get; set; }
    }
}
