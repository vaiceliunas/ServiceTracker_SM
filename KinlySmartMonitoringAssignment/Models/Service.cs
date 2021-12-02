using System;
using System.Collections.Generic;

#nullable disable

namespace KinlySmartMonitoringAssignment.Models
{
    public partial class Service
    {
        public Service()
        {
            Labels = new HashSet<Label>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Port { get; set; }
        public string MaintainerEmail { get; set; }

        public virtual ICollection<Label> Labels { get; set; }
    }
}
