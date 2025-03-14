using System;
using System.Collections.Generic;

namespace P_Cloud_API.Models
{
    public partial class Area
    {
        public Area()
        {
            ProcessCells = new HashSet<ProcessCell>();
        }

        public int Id { get; set; }
        public int? SiteId { get; set; }
        public string? Name { get; set; }

        public virtual Site? Site { get; set; }
        public virtual ICollection<ProcessCell> ProcessCells { get; set; }
    }
}
