using System;
using System.Collections.Generic;

namespace P_Cloud_API.Models
{
    public partial class Site
    {
        public Site()
        {
            Areas = new HashSet<Area>();
        }

        public int Id { get; set; }
        public int? EnterpriseId { get; set; }
        public string? Name { get; set; }

        public virtual Enterprise? Enterprise { get; set; }
        public virtual ICollection<Area> Areas { get; set; }
    }
}
