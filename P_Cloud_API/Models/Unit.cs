using System;
using System.Collections.Generic;

namespace P_Cloud_API.Models
{
    public partial class Unit
    {
        public Unit()
        {
            EquipmentModules = new HashSet<EquipmentModule>();
        }

        public int Id { get; set; }
        public int? ProcessCellId { get; set; }
        public string? Name { get; set; }

        public virtual ProcessCell? ProcessCell { get; set; }
        public virtual ICollection<EquipmentModule> EquipmentModules { get; set; }
    }
}
