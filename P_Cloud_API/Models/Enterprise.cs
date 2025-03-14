using System;
using System.Collections.Generic;

namespace P_Cloud_API.Models
{
    public partial class Enterprise
    {
        public Enterprise()
        {
            Sites = new HashSet<Site>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Site> Sites { get; set; }
    }
}
