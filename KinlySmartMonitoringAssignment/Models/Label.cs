using System;
using System.Collections.Generic;

#nullable disable

namespace KinlySmartMonitoringAssignment.Models
{
    public partial class Label
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string LabelKey { get; set; }
        public string LabelValue { get; set; }
    }
}
