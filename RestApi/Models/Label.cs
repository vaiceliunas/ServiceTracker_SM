using System;
using System.Collections.Generic;
using KinlySmartMonitoringAssignment.Models;

#nullable disable

namespace RestApi.Models
{
    public partial class Label
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string LabelKey { get; set; }
        public string LabelValue { get; set; }
    }
}
