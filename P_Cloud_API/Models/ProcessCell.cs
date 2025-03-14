using System;
using System.Collections.Generic;

namespace P_Cloud_API.Models
{
    public partial class ProcessCell
    {
        public ProcessCell()
        {
            Units = new HashSet<Unit>();
        }

        public int Id { get; set; }
        public int? AreaId { get; set; }
        public string? Name { get; set; }

        public virtual Area? Area { get; set; }
        public virtual ICollection<Unit> Units { get; set; }
    }
}
