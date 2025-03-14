using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace P_Cloud_API.Models
{
    public partial class ControlModuleInfo
    {
        public ControlModuleInfo()
        {
            ControlModules = new HashSet<ControlModule>();
        }

        public int Id { get; set; }
        public int? ControlModuleId { get; set; }
        public DateTime? Timestamp { get; set; }
        public string? UserIp { get; set; }
        public string? Username { get; set; }
        public int? EditTypeId { get; set; }
        public int? StatusId { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public decimal? Tolerance { get; set; }
        public decimal? RangeLowerEnd { get; set; }
        public decimal? RangeUpperEnd { get; set; }
        public int? PhysicalUnitId { get; set; }

        public virtual ControlModule? ControlModule { get; set; }
        public virtual EditType? EditType { get; set; }
        public virtual PhysicalUnit? PhysicalUnit { get; set; }
        public virtual Status? Status { get; set; }

        [JsonIgnore]
        public virtual ICollection<ControlModule> ControlModules { get; set; }
    }
}
