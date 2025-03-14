using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace P_Cloud_API.Models
{
    public partial class ProcessData
    {
        public int Id { get; set; }
        public int? ControlModuleId { get; set; }
        public DateTime? Timestamp { get; set; }
        public string? UserIp { get; set; }
        public string? Username { get; set; }
        public int? EditTypeId { get; set; }
        public int? StatusId { get; set; }
        public string? StatusMessage { get; set; }
        public bool? Error { get; set; }
        public decimal? CurrentValue { get; set; }

        [JsonIgnore]
        public virtual ControlModule? ControlModule { get; set; }
        public virtual EditType? EditType { get; set; }
        public virtual Status? Status { get; set; }
    }
}
